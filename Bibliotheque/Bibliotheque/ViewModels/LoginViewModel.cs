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
    public class LoginViewModel : BindableBase, INavigationAware
    {
        private readonly IMapper m_Mapper;
        private readonly ILibraryRepository m_Repository;
        private readonly IUserService m_UserService;

        private IRegionNavigationService m_Region;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand NavigateToRegisterCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

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

        private string m_MessageErreur;

        public string MessageErreur
        {
            get { return m_MessageErreur; }
            set { SetProperty(ref m_MessageErreur, value); }
        }

        public string Title => throw new NotImplementedException();

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

        public async Task<bool> LoginRequest(LoginModel login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                MessageErreur = "Veuillez entrer les informations dans les champs requis!";
                return false;
            }

            var response = m_UserService.Login(m_Mapper.Map<LoginRequest>(login));
            if (response == null)
            {
                MessageErreur = "Les informations de connexion sont incorrects!";
                return false;
            }

            if (await m_Repository.UserIsBlackListed(response.UserId))
            {
                MessageErreur = "Vous êtes interdit dans cette bibliothèque!";
                return false;
            }

            using (StreamWriter writer = File.CreateText("UserConnection"))
            {
                foreach (PropertyInfo prop in response.GetType().GetProperties())
                {
                    await writer.WriteLineAsync(prop.ToString());
                }
            }
            return true;
        }

        public async Task Login()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                MessageErreur = "L'email et le mot de passe sont nécessaires à la connexion!";
            if (!EmailHelper.IsValidEmail(Email))
                MessageErreur = "Le format de l'email est invalide!";

            LoginModel login = new()
            {
                Email = this.Email,
                Password = this.Password
            };

            if (await LoginRequest(login))
            {

                // TODO : Renvoyer vers la dernière page visitée
            }
            else
            {
                MessageErreur = "Le mail ou le mot de passe sont incorrect.";
            }
        }

        public void NavigateToRegister()
        {
            if (m_Region == null)
                throw new ArgumentNullException(nameof(m_Region));
            m_Region.RequestNavigate("RegisterView");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            m_Region = navigationContext.Parameters.GetValue<IRegionNavigationService>("Region");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
