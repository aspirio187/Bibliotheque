using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.EntityFramework.StaticData;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class AdminUsersViewModel : BaseViewModel
    {
        public enum FiltersEnum
        {
            Users,
            Moderators,
            Admins,
            SuperAdmins,
            Blackliste,
            Autorise,
        }
        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand BlackListCommand { get; set; }
        public DelegateCommand AuthorizeCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand AuthorizeModificationCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/
        private ObservableCollection<UserAdminModel> m_Users;

        public ObservableCollection<UserAdminModel> Users
        {
            get => m_Users;
            set => SetProperty(ref m_Users, value);
        }

        private ObservableCollection<FiltersEnum> m_Filters;

        public ObservableCollection<FiltersEnum> Filters
        {
            get { return m_Filters; }
            set { SetProperty(ref m_Filters, value); }
        }

        private ObservableCollection<string> m_Roles;

        public ObservableCollection<string> Roles
        {
            get { return m_Roles; }
            set { SetProperty(ref m_Roles, value); }
        }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue      
        private bool m_UserModification = true;

        public bool UserModification
        {
            get { return m_UserModification; }
            set { SetProperty(ref m_UserModification, value); }
        }

        private FiltersEnum m_Filter;

        public FiltersEnum Filter
        {
            get { return m_Filter; }
            set
            {
                Task.Run(() => FilterUsers(value));
                SetProperty(ref m_Filter, value);
            }
        }

        private DateTime m_EndBlackList = DateTime.Now;

        public DateTime EndBlackList
        {
            get { return m_EndBlackList; }
            set { SetProperty(ref m_EndBlackList, value); }
        }

        private string m_SearchText;

        public string SearchText
        {
            get { return m_SearchText; }
            set { SetProperty(ref m_SearchText, value); }
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public AdminUsersViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            Filters = new(Enum.GetValues<FiltersEnum>());
            Roles = new(Enum.GetNames<RolesEnum>());

            BlackListCommand = new(async () => await BlackList());
            AuthorizeCommand = new(async () => await Authorize());
            SearchCommand = new(async () => await Search());
            AuthorizeModificationCommand = new(AuthorizeModification);
            SaveCommand = new(async () => await Save());
            LoadCommand = new(async () => await LoadAsync());

        }

        public async Task FilterUsers(FiltersEnum filter)
        {
            await LoadAsync();
            switch (filter)
            {
                case FiltersEnum.Users:
                    Users = new(Users.Where(u => u.Role.Equals(RolesEnum.User.ToString())));
                    break;
                case FiltersEnum.Moderators:
                    Users = new(Users.Where(u => u.Role.Equals(RolesEnum.Moderator.ToString())));
                    break;
                case FiltersEnum.Admins:
                    Users = new(Users.Where(u => u.Role.Equals(RolesEnum.Admin.ToString())));
                    break;
                case FiltersEnum.SuperAdmins:
                    Users = new(Users.Where(u => u.Role.Equals(RolesEnum.SuperAdmin.ToString())));
                    break;
                case FiltersEnum.Blackliste:
                    Users = new(Users.Where(u => u.BlackListed == true));
                    break;
                case FiltersEnum.Autorise:
                    Users = new(Users.Where(u => u.BlackListed == false));
                    break;
            }
        }

        public async Task BlackList()
        {
            if (EndBlackList > DateTime.Now)
            {
                foreach (var user in Users)
                {
                    if (user.Selected == true)
                    {
                        var blackListFromRepo = await m_Repository.GetBlackList(user.Id);

                        if (blackListFromRepo is null)
                        {
                            m_Repository.BlackListUser(user.Id, EndBlackList);
                        }
                        else
                        {
                            blackListFromRepo.EndDate = EndBlackList;
                        }
                    }
                }
                await m_Repository.SaveAsync();
                LoadCommand.Execute();
            }
        }

        public async Task Authorize()
        {
            foreach (var user in Users)
            {
                if (user.BlackListed && user.Selected)
                {
                    m_Repository.AuthorizeUser(user.Id);
                }
            }
            await m_Repository.SaveAsync();
            LoadCommand.Execute();
        }

        public async Task Search()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                await LoadAsync();
                Users = new(Users.Where(
                    u => u.FirstName.ToLower().Contains(SearchText.ToLower()) ||
                        u.LastName.ToLower().Contains(SearchText.ToLower()) ||
                        u.PhoneNumber.ToLower().Contains(SearchText.ToLower()) ||
                        u.FullAddress.ToLower().Contains(SearchText.ToLower()) ||
                        u.FullCity.ToLower().Contains(SearchText.ToLower()) ||
                        u.Email.ToLower().Contains(SearchText.ToLower())));
            }
        }

        public void AuthorizeModification()
        {
            UserModification = !UserModification;
        }

        public async Task Save()
        {
            foreach (var user in Users)
            {
                if (user.IsValid())
                {
                    var userFromRepo = await m_Repository.GetUserAsync(user.Id);
                    var userRole = await m_Repository.GetRole(user.Role);
                    if (userRole is not null)
                    {
                        userFromRepo.RoleId = userRole.Id;
                        userFromRepo.Role = userRole;
                    }

                    var userAddress = await m_Repository.GetUserAddress(user.Id);
                    if (userAddress is not null)
                    {
                        string[] streetAppart = user.FullAddress.Split('-');
                        string[] cityZip = user.FullCity.Split('-');
                        if (streetAppart.Length != 2 || cityZip.Length != 2) break;
                        AddressModel address = new()
                        {
                            Id = userAddress.Id,
                            Street = streetAppart[0],
                            Appartment = streetAppart[1],
                            City = cityZip[0],
                            ZipCode = cityZip[1]
                        };
                        m_Mapper.Map(address, userAddress);
                        userFromRepo.AddressId = userAddress.Id;
                    }
                    m_Mapper.Map(user, userFromRepo);
                }
            }
            await m_Repository.SaveAsync();
        }

        public override async Task LoadAsync()
        {
            Users = new(m_Mapper.Map<IEnumerable<UserAdminModel>>(await m_Repository.GetUsersAsync(true, true)));
            foreach (var user in Users)
            {
                if (await m_Repository.UserIsBlackListed(user.Id))
                {
                    user.BlackListed = true;
                }
            }
        }
    }
}
