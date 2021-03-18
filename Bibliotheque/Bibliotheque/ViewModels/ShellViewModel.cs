using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bibliotheque.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly RegionManager m_RegionManager;
        private readonly IRegionNavigationService m_NavigationService;
        private readonly ILibraryRepository m_Repository;

        private readonly string RegionName = "CustomView";

        /***************************************************/
        /**************** Propriétés objets ****************/
        /***************************************************/

        public UserCurrectSessionRecord CurrentSession { get; set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand NavigateToProfileCommand { get; set; }
        public DelegateCommand GoBackCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        private bool m_IsConnected = false;

        public bool IsConnected
        {
            get { return m_IsConnected; }
            set { SetProperty(ref m_IsConnected, value); }
        }


        public ShellViewModel(IRegionManager regionManager, IRegionNavigationService navigationService, ILibraryRepository repository)
        {
            // Instanciation des propriétés
            m_RegionManager = (RegionManager)regionManager ??
                throw new ArgumentNullException(nameof(regionManager));
            m_NavigationService = navigationService ??
                throw new ArgumentNullException(nameof(navigationService));
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            // Initialisation des commandes
            NavigateToProfileCommand = new(NavigateToProfile);
            GoBackCommand = new(GoBack);

            // Vérifie si l'utilisateur a déjà une session en cours
            Task.Run(UserIsConnected);
        }

        public async Task UserIsConnected()
        {
            if (File.Exists(GlobalInfos.UserSessionPath))
            {
                CurrentSession = await LocalFileHelper.ReadJsonFile<UserCurrectSessionRecord>(GlobalInfos.UserSessionPath);
                if (CurrentSession != null)
                {
                    if (!await m_Repository.UserTokenHasChanged(CurrentSession.Id, CurrentSession.Token))
                        IsConnected = true;
                    else
                        File.Delete(GlobalInfos.UserSessionPath);
                }
            }
        }

        public void NavigateToProfile()
        {
            if (IsConnected)
            {
                // TODO : Navigate to profile page
            }
            else
            {
                IRegion mainRegion = m_RegionManager.Regions[RegionName];
                var parameters = new NavigationParameters();
                m_NavigationService.Region = mainRegion;
                parameters.Add(GlobalInfos.NavigationServiceName, m_NavigationService);
                m_NavigationService.RequestNavigate(new Uri(GlobalInfos.LoginView, UriKind.Relative), parameters);
            }
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
