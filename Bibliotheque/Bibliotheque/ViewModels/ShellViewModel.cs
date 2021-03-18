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

        private readonly string RegionName = "MainRegion";

        /***************************************************/
        /**************** Propriétés objets ****************/
        /***************************************************/

        public UserCurrectSessionRecord CurrentSession { get; set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand NavigateToProfileCommand { get; set; }
        public DelegateCommand NavigateToHomeCommand { get; set; }
        public DelegateCommand NavigateToCollectionCommand { get; set; }
        public DelegateCommand NavigateToAdminPanelCommand { get; set; }

        public DelegateCommand ExitCommand { get; set; }

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
            // Défini la Region dans laquelle se passeront toutes les navigations
            m_NavigationService.Region = m_RegionManager.Regions[RegionName];
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            // Initialisation des commandes
            GoBackCommand = new(GoBack);
            NavigateToProfileCommand = new(NavigateToProfile);

            // Vérifie si l'utilisateur a déjà une session en cours
            Task.Run(UserIsConnected);

            // TODO : Regrouper ces instructions de navigation dans une méthode avec comme parameter ViewsEnum
            var parameters = new NavigationParameters();
            parameters.Add(GlobalInfos.NavigationServiceName, m_NavigationService);
            m_NavigationService.RequestNavigate(new Uri(GlobalInfos.HomeView, UriKind.Relative), parameters);
        }

        /// <summary>
        /// Méthode asynchrone qui vérifie si l'utilisateur a une session enregistrée
        /// </summary>
        /// <returns>
        /// true Si une session enregistrée existe et le token de la session est le même que celui de l'utilisateur
        /// dans le contexte. false Dans le cas contraire
        /// </returns>
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

        /// <summary>
        /// Méthode de navigation vers le profil de l'utilisateur. Si l'utilisateur a une session, il sera redirigé vers sa
        /// page profil, sinon il sera redirigé vers la page de connexion.
        /// </summary>
        public void NavigateToProfile()
        {
            if (IsConnected)
            {
                // TODO : Navigate to profile page
            }
            else
            {
                Navigate(ViewsEnum.LoginView);
            }
        }

        /// <summary>
        /// Méthode de navigation qui prend en paramètre une vue de l'enum ViewsEnum et un dictionnaire pour les paramètres.
        /// Le dictionnaire prend comme Key un string représentant le nom de l'objet à transférer et Value l'objet en question.
        /// </summary>
        /// <param name="view">Element de l'enum ViewsEnum</param>
        /// <param name="navigationParams"></param>
        public void Navigate(ViewsEnum view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { GlobalInfos.NavigationServiceName, m_NavigationService },
                { GlobalInfos.ConnectionName, IsConnected }
            };

            if (navigationParams != null)
            {
                foreach (var navigationParam in navigationParams)
                {
                    navigationParameters.Add(navigationParam.Key, navigationParam.Value);
                }
            }
            m_NavigationService.RequestNavigate(new Uri(view.ToString(), UriKind.Relative), navigationParameters);
        }

        /// <summary>
        /// Fonction qui renvoi en arrière dans le stack de navigation si le retour est possible.
        /// </summary>
        public void GoBack()
        {
            if (m_NavigationService.Journal.CanGoBack)
            {
                m_NavigationService.Journal.GoBack();
            }
        }

        /// <summary>
        /// Quitte l'application
        /// </summary>
        /// <returns></returns>
        public async Task Exit()
        {
            await m_Repository.SaveAsync();
            Application.Current.Shutdown();
        }
    }
}
