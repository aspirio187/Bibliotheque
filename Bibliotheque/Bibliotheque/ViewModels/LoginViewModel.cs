using AutoMapper;
using Bibliotheque.EntityFramework.Services.Authentication;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Bibliotheque.UI.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware, IJournalAware
    {
        private readonly IMapper m_Mapper;
        private readonly ILibraryRepository m_Repository;
        private readonly IUserService m_UserService;

        private IRegionNavigationService m_Navigation;

        public bool IsRelevant { get; set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand NavigateToRegisterCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private string m_Email;

        public string Email
        {
            get { return m_Email; }
            set { SetProperty(ref m_Email, value); }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set { SetProperty(ref m_Password, value); }
        }

        private string m_ErrorMessage;

        public string ErrorMessage
        {
            get { return m_ErrorMessage; }
            set { SetProperty(ref m_ErrorMessage, value); }
        }
        #endregion

        public LoginViewModel(ILibraryRepository repository, IMapper mapper, IUserService userService)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            m_UserService = userService ??
                throw new ArgumentNullException(nameof(userService));

            LoginCommand = new DelegateCommand(async () => await Login());
            NavigateToRegisterCommand = new DelegateCommand(NavigateToRegister);
        }

        /// <summary>
        /// "Demande" la connexion grâce aux identifiants fournis en paramètre.
        /// </summary>
        /// <param name="login">
        /// Record utilisé pour que l'utilisateur puisse se connecter
        /// </param>
        /// <returns>
        /// true Si la connexion est réussie. false Dans le cas contraire
        /// </returns>
        public async Task<bool> LoginRequest(UserConnectionRecord login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                ErrorMessage = "Veuillez entrer les informations dans les champs requis!";
                return false;
            }

            var response = m_UserService.Login(m_Mapper.Map<LoginRequest>(login));
            if (response == null)
            {
                ErrorMessage = "Les informations de connexion sont incorrect!";
                return false;
            }

            if (await m_Repository.UserIsBlackListed(response.Id))
            {
                ErrorMessage = "Vous êtes interdit dans cette bibliothèque!";
                return false;
            }

            if (!await LocalFileHelper.WriteJsonFile(GlobalInfos.UserSessionPath, response))
            {
                ErrorMessage = "Une erreur est survenue lors de la connexion";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tente d'authentifier et connecter l'utilisateur
        /// </summary>
        public async Task Login()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                ErrorMessage = "L'email et le mot de passe sont nécessaires à la connexion!";
            if (!EmailHelper.IsValidEmail(Email))
                ErrorMessage = "Le format de l'email est invalide!";

            UserConnectionRecord login = new(Email, Password);

            if (await LoginRequest(login))
            {
                // TODO : Renvoyer vers la dernière page visitée
                IsRelevant = false;
                GoBack();
            }
        }

        /// <summary>
        /// Renvoi vers la page précédente
        /// </summary>
        private void GoBack()
        {
            if (m_Navigation.Journal.CanGoBack)
            {
                m_Navigation.Journal.GoBack();
            }
        }

        public void NavigateToRegister()
        {
            if (m_Navigation == null) throw new ArgumentNullException(nameof(m_Navigation));
            IsRelevant = true;
            var parameters = new NavigationParameters();
            parameters.Add(GlobalInfos.NavigationServiceName, m_Navigation);
            m_Navigation.RequestNavigate(new Uri(GlobalInfos.RegisterView, UriKind.Relative), parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (m_Navigation == null) m_Navigation = navigationContext.Parameters.GetValue<IRegionNavigationService>(GlobalInfos.NavigationServiceName);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public bool PersistInHistory()
        {
            return true;
        }
    }
}
