using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.EntityFramework.StaticData;
using Bibliotheque.UI.DefaultData;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "AdminRegion";

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand NavigateToAdminBooksCommand { get; set; }
        public DelegateCommand NavigateToAdminUsersCommand { get; set; }
        public DelegateCommand NavigateToAdminBlackListCommand { get; set; }

        public AdminViewModel(ILibraryRepository repository, IMapper mapper, IRegionManager region)
            : base(repository, mapper)
        {
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));

            LoadCommand = new(Load);
            NavigateToAdminBooksCommand = new(NavigateToAdminBooks);
            NavigateToAdminUsersCommand = new(NavigateToAdminUsers);
            NavigateToAdminBlackListCommand = new(NavigateToAdminBlackList);
        }

        public override void Load()
        {
            NavigateToAdminBooks();
        }

        public void NavigateToAdminBooks()
        {
            Navigate(ViewsEnum.AdminBooksView);
        }

        public void NavigateToAdminUsers()
        {
            Navigate(ViewsEnum.AdminUsersView);
        }

        public void NavigateToAdminBlackList()
        {
            Navigate(ViewsEnum.AdminBlackListView);
        }


        public override void Navigate(ViewsEnum view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { GlobalInfos.CurrentSession, CurrentSession },
                { GlobalInfos.NavigationService, m_NavigationService },
                { m_RegionName, m_Region }
            };

            if (navigationParams != null)
            {
                foreach (var navigationParam in navigationParams)
                {
                    navigationParameters.Add(navigationParam.Key, navigationParam.Value);
                }
            }
            m_Region.RequestNavigate(m_RegionName, new Uri(view.ToString(), UriKind.Relative), navigationParameters);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (CurrentSession is null)
            {
                Navigate(ViewsEnum.HomeView);
                m_NavigationService.Journal.Clear();
            }
            else
            {
                RolesEnum userRole = Task.Run(() => m_Repository.GetUserRole(CurrentSession.Id)).Result;
            }
        }
    }
}
