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
    public class AdminViewModel : BindableBase, INavigationAware
    {
        public enum AdminViews
        {
            AdminBooksView,
            AdminUsersView,
            AdminBlackListView
        }

        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "AdminRegion";

        private IRegionNavigationService m_Navigation;
        private UserCurrentSessionRecord m_CurrentSession;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand NavigateToAdminBooksCommand { get; set; }
        public DelegateCommand NavigateToAdminUsersCommand { get; set; }
        public DelegateCommand NavigateToAdminBlackListCommand { get; set; }

        public AdminViewModel(IRegionManager region)
        {
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));

            LoadCommand = new(Load);
            NavigateToAdminBooksCommand = new(NavigateToAdminBooks);
            NavigateToAdminUsersCommand = new(NavigateToAdminUsers);
            NavigateToAdminBlackListCommand = new(NavigateToAdminBlackList);
        }

        public void Load()
        {
            NavigateToAdminBooks();
        }

        public void NavigateToAdminBooks()
        {
            Navigate(AdminViews.AdminBooksView);
        }

        public void NavigateToAdminUsers()
        {
            Navigate(AdminViews.AdminUsersView);
        }

        public void NavigateToAdminBlackList()
        {
            Navigate(AdminViews.AdminBlackListView);
        }


        public void Navigate(AdminViews view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { NavParameters.CurrentSessionParam, m_CurrentSession }
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


        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (m_Navigation == null) m_Navigation = navigationContext.Parameters.GetValue<IRegionNavigationService>(GlobalInfos.NavigationService);
            if (m_CurrentSession == null) m_CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(NavParameters.CurrentSessionParam);
            if (m_CurrentSession == null)
            {
                m_Navigation.Journal.GoBack();
                m_Navigation.Journal.Clear();
            }
        }
    }
}
