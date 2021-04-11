using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.EntityFramework.StaticData;
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
            Blackliste,
            Autorise,
        }
        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand BlackListCommand { get; set; }
        public DelegateCommand AuthorizeCommand { get; set; }
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
        private bool m_UserModification;

        public bool UserModification
        {
            get { return m_UserModification; }
            set { SetProperty(ref m_UserModification, value); }
        }

        private string m_Filter;

        public string Filter
        {
            get { return m_Filter; }
            set
            {
                SetProperty(ref m_Filter, value);
            }
        }

        private DateTime m_EndBlackList;

        public DateTime EndBlackList
        {
            get { return m_EndBlackList; }
            set { SetProperty(ref m_EndBlackList, value); }
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
            AuthorizeModificationCommand = new(AuthorizeModification);
            LoadCommand = new(async () => await LoadAsync());

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
            }
        }

        public async Task Authorize()
        {
            foreach (var user in Users)
            {
                if (user.BlackListed)
                {
                    m_Repository.AuthorizeUser(user.Id);
                }
            }
            await m_Repository.SaveAsync();
        }

        public void AuthorizeModification()
        {
            if (UserModification == true)
            {
                UserModification = false;
            }
            else
            {
                UserModification = true;
            }
        }

        public async Task Save()
        {
            foreach (var user in Users)
            {
                if (user.IsValid())
                {
                    var userFromRepo = await m_Repository.GetUserAsync(user.Id);
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
