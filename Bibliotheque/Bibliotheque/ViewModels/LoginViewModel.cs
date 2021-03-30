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
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService m_UserService;

        private UserConnection UserConnection { get; }

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
            set
            {
                var result = UserConnection.DefineEmail(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Email, value);
            }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set
            {
                var result = UserConnection.DefinePassword(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Password, value);
            }
        }
        #endregion

        public LoginViewModel(ILibraryRepository repository, IMapper mapper, IUserService userService)
            : base(repository, mapper)
        {
            m_UserService = userService ??
                throw new ArgumentNullException(nameof(userService));

            UserConnection = new();

            // Initialisation des commandes
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
        public async Task<bool> LoginRequest()
        {
            var response = m_UserService.Login(m_Mapper.Map<LoginRequest>(UserConnection));
            bool responseResult = response is null ? false : true;
            CheckError("Reponse", "Les informations de connexions sont incorrects !", responseResult);

            if (responseResult == false) return false;
            bool blackListed = await m_Repository.UserIsBlackListed(response.Id);
            CheckError("Black listé", "Vous êtes actuellement sur la liste noire !", blackListed);
            if (blackListed) return false;
            bool writeFile = await LocalFileHelper.WriteJsonFile(GlobalInfos.UserSessionPath, response);
            CheckError("Session", "Une erreur inconnue est survenue lors de la connexion !", writeFile);
            if (writeFile == false) return false;
            return true;
        }

        /// <summary>
        /// Tente d'authentifier et connecter l'utilisateur
        /// </summary>
        public async Task Login()
        {
            bool UserIsValid = UserConnection.IsValid();
            CheckError("Validité", "Les informations de connexion sont invalides !", UserIsValid);
            if (UserIsValid)
            {
                bool loginResult = await LoginRequest();
                CheckError("Connexion", "Une erreur est survenue lors de la connexion !", loginResult);
                if (loginResult)
                {
                    GoBack();
                }
            }
        }

        public void NavigateToRegister()
        {
            Navigate(ViewsEnum.RegisterView);
        }
    }
}
