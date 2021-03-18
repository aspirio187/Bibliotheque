using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Helpers;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ProfileViewModel : BindableBase, INavigationAware
    {
        internal enum ProfileViews
        {
            ProfileInformationsView,
        }

        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;
        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "ProfileRegion";

        private IRegionNavigationService m_Navigation;

        public ProfileViewModel(ILibraryRepository repository, IMapper mapper, IRegionManager region)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));
        }

        public void NavigateToProfileInformations()
        {
            Navigate(ProfileViews.ProfileInformationsView);
        }

        public void Navigate(ProfileViews view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                // TODO : Ajouter les paramètres nécessaires
            };

            if (navigationParams != null)
            {
                foreach (var navigationParam in navigationParams)
                {
                    navigationParameters.Add(navigationParam.Key, navigationParam.Value);
                }
            }
            m_NavigationService.RequestNavigate(new Uri(view.ToString(), UriKind.Relative), navigationParameters);
            m_Region.RequestNavigate(m_RegionName,new Uri(view.ToString(), UriKind.Relative),)
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (m_Navigation == null) m_Navigation = navigationContext.Parameters.GetValue<IRegionNavigationService>(GlobalInfos.NavigationServiceName);
        }
    }
}
