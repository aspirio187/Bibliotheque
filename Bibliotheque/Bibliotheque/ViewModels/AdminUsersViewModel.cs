using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
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
        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/
        private ObservableCollection<UserAdminModel> m_Users;

        public ObservableCollection<UserAdminModel> Users
        {
            get => m_Users;
            set => SetProperty(ref m_Users, value);
        }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

        public AdminUsersViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            LoadCommand = new(async () => await LoadAsync());
        }

        public override async Task LoadAsync()
        {
            Users = new(m_Mapper.Map<IEnumerable<UserAdminModel>>(await m_Repository.GetUsersAsync(true, true)));

        }
    }
}
