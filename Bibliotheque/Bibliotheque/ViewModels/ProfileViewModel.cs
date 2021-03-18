using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
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
    public class ProfileViewModel : BindableBase, INavigationAware, IJournalAware
    {
        public enum ProfileViews
        {
            ProfileInformationsView,
            ProfileAddressView,
            ProfilePasswordView,
            ProfileBorrowView,
            ProfileHistoryView
        }

        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;
        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "ProfileRegion";

        private IRegionNavigationService m_Navigation;
        private UserCurrentSessionRecord m_CurrentSession;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand NavigateToProfileInformationsCommand { get; set; }
        public DelegateCommand NavigateToAddressCommand { get; set; }
        public DelegateCommand NavigateToPasswordCommand { get; set; }
        public DelegateCommand NavigateToBorrowsCommand { get; set; }
        public DelegateCommand NavigateToHistoryCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

        public ProfileViewModel(ILibraryRepository repository, IMapper mapper, IRegionManager region)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));

            // Déclarations des commandes
            LoadCommand = new(Load);
            NavigateToProfileInformationsCommand = new(NavigateToProfileInformations);
            NavigateToAddressCommand = new(NavigateToAddress);
            NavigateToPasswordCommand = new(NavigateToPassword);
            NavigateToBorrowsCommand = new(NavigateToBorrows);
            NavigateToHistoryCommand = new(NavigateToHistory);
        }

        public void Load()
        {
            Navigate(ProfileViews.ProfileInformationsView);
        }

        public void NavigateToProfileInformations()
        {
            Navigate(ProfileViews.ProfileInformationsView);
        }

        public void NavigateToAddress()
        {
            Navigate(ProfileViews.ProfileAddressView);
        }

        public void NavigateToPassword()
        {
            Navigate(ProfileViews.ProfilePasswordView);
        }

        public void NavigateToBorrows()
        {
            Navigate(ProfileViews.ProfileBorrowView);
        }

        public void NavigateToHistory()
        {
            Navigate(ProfileViews.ProfileHistoryView);
        }

        /// <summary>
        /// Navigue vers la page de profile définie par l'élément de l'enum ProfileViews qui représente
        /// le nom d'une ProfileView
        /// </summary>
        /// <param name="view">
        /// Vue désirée
        /// </param>
        /// <param name="navigationParams">
        /// Dictionnaire de paramètre passable à la vue
        /// </param>
        public void Navigate(ProfileViews view, Dictionary<string, object> navigationParams = null)
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
            if (m_Navigation == null) m_Navigation = navigationContext.Parameters.GetValue<IRegionNavigationService>(GlobalInfos.NavigationServiceName);
            if (m_CurrentSession == null) m_CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(NavParameters.CurrentSessionParam);
            if (m_CurrentSession == null)
            {
                m_Navigation.Journal.GoBack();
                m_Navigation.Journal.Clear();
            }
        }

        public bool PersistInHistory()
        {
            return false;
        }
    }
}
