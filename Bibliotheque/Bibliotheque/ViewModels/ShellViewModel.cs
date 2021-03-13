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
    public class ShellViewModel : BindableBase
    {
        private readonly RegionManager m_RegionManager;
        private readonly IRegionNavigationService m_NavigationService;

        public DelegateCommand NavigateToProfileCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }

        public ShellViewModel(IRegionManager regionManager, IRegionNavigationService navigationService)
        {
            m_RegionManager = (RegionManager)regionManager ??
                throw new ArgumentNullException(nameof(regionManager));
            m_NavigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));

            NavigateToProfileCommand = new(NavigateToProfile);
            GoBackCommand = new(GoBack);
        }

        public void NavigateToProfile()
        {
            IRegion mainRegion = m_RegionManager.Regions["CustomView"];
            //mainRegion.RequestNavigate("LoginView");
            var parameters = new NavigationParameters();
            //m_RegionManager.RequestNavigate("CustomView", "LoginView", parameters);
            m_NavigationService.Region = mainRegion;
            parameters.Add("Region", m_NavigationService);
            m_NavigationService.RequestNavigate(new Uri("LoginView", UriKind.Relative), parameters);
        }

        public void GoBack()
        {
            if (m_NavigationService.Journal.CanGoBack)
            {
                m_NavigationService.Journal.GoBack();
            }
        }
    }
}
