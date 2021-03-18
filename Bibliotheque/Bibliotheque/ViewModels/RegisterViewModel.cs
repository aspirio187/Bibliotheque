using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Helpers;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.DefaultData;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Bibliotheque.UI.ViewModels
{
    /// <summary>
    /// Toutes les différentes erreurs que l'on peut rencontrer pendant l'enregistrement d'un utilisateur
    /// </summary>
    public enum RegisterErrors
    {
        InvalidEmail,
        InvalidEmailConfirmation,
        EmailsDifferent,
        InvalidePassword,
        InvalidPasswordConfirmation,
        PasswordsDifferent,
        InvalidField,
        InvalidDate,
        AgeMinimum,
        InvalidZipCode,
        InvalidPhone,
        AddressCreation,
        UserCreation
    }

    /// <summary>
    /// Objet 'Error'
    /// </summary>
    public record ErrorRecord(RegisterErrors ErrorType, PropertyInfo Property, string ErrorMessage);

    public class RegisterViewModel : BindableBase, INavigationAware, IJournalAware
    {
        /// <summary>
        /// Enumeration répertoriant toutes les propriétés liées à la vue dans ce ViewModel
        /// </summary>
        public enum Properties
        {
            Email,
            EmailConfirmation,
            Password,
            PasswordConfirmation,
            FirstName,
            LastName,
            Gender,
            BirthDate,
            Street,
            ZipCode,
            City,
            PhoneNumber
        }

        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;

        private IRegionNavigationService m_Navigation;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand RegisterCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        private ObservableCollection<ErrorRecord> m_ErrorsList = new();

        public ObservableCollection<ErrorRecord> ErrorsList
        {
            get { return m_ErrorsList; }
            set
            {
                SetProperty(ref m_ErrorsList, value);
            }
        }

        private ObservableCollection<GenderRecord> m_GendersList;

        public ObservableCollection<GenderRecord> GendersList
        {
            get { return m_GendersList; }
            set { SetProperty(ref m_GendersList, value); }
        }


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
                if (!EmailIsValid(value))
                    RaiseError(RegisterErrors.InvalidEmail, Properties.Email, "L'addresse email entrée n'est pas valide!");
                else
                    ClearError(RegisterErrors.InvalidEmail, Properties.Email);

                SetProperty(ref m_Email, value);
            }
        }

        private string m_EmailConfirmation;

        public string EmailConfirmation
        {
            get { return m_EmailConfirmation; }
            set
            {
                if (!EmailIsValid(value))
                    RaiseError(RegisterErrors.InvalidEmailConfirmation, Properties.EmailConfirmation, "L'adresse email de confirmation n'est pas valide");
                else
                    ClearError(RegisterErrors.InvalidEmailConfirmation, Properties.EmailConfirmation);

                if (!value.Equals(Email))
                    RaiseError(RegisterErrors.EmailsDifferent, Properties.EmailConfirmation, "L'adresse email de confirmation est différente de l'addresse email!");
                else
                    ClearError(RegisterErrors.EmailsDifferent, Properties.EmailConfirmation);

                SetProperty(ref m_EmailConfirmation, value);
            }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set
            {
                if (!PasswordIsValid(value))
                    RaiseError(RegisterErrors.InvalidePassword,
                        Properties.Password,
                        "Le mot de passe doit faire entre 8 et 25 caractères et contenir au moins une lettre minuscule, une lettre majuscule, un chiffre et un caractère spécial");
                else
                    ClearError(RegisterErrors.InvalidePassword, Properties.Password);

                SetProperty(ref m_Password, value);
            }
        }

        private string m_PasswordConfirmation;

        public string PasswordConfirmation
        {
            get { return m_PasswordConfirmation; }
            set
            {
                if (!PasswordIsValid(value))
                    RaiseError(RegisterErrors.InvalidPasswordConfirmation,
                        Properties.PasswordConfirmation,
                        "Le mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule, un chiffre et un caractère spécial");
                else
                    ClearError(RegisterErrors.InvalidPasswordConfirmation, Properties.PasswordConfirmation);

                if (!value.Equals(Password))
                    RaiseError(RegisterErrors.PasswordsDifferent,
                        Properties.PasswordConfirmation,
                        "Les mots de passes ne concordent pas!");
                else
                    ClearError(RegisterErrors.PasswordsDifferent, Properties.PasswordConfirmation);

                SetProperty(ref m_PasswordConfirmation, value);
            }
        }

        private string m_FirstName;

        public string FirstName
        {
            get { return m_FirstName; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                    RaiseError(RegisterErrors.InvalidField, Properties.FirstName, "Veuillez fournir le prénom!");
                else
                    ClearError(RegisterErrors.InvalidField, Properties.FirstName);

                SetProperty(ref m_FirstName, value.Trim().ToLower().FirstCharToUpper());
            }
        }

        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                    RaiseError(RegisterErrors.InvalidField, Properties.LastName, "Veuillez fournir le nom de famille!");
                else
                    ClearError(RegisterErrors.InvalidField, Properties.LastName);
                SetProperty(ref m_LastName, value.Trim().ToLower().FirstCharToUpper());
            }
        }

        private GenderRecord m_Gender;

        public GenderRecord Gender
        {
            get { return m_Gender; }
            set { SetProperty(ref m_Gender, value); }
        }

        private DateTime m_BirthDate;

        public DateTime BirthDate
        {
            get { return m_BirthDate; }
            set
            {
                if (value >= DateTime.Now)
                    RaiseError(RegisterErrors.InvalidDate, Properties.BirthDate, "La date fournie est invalide!");
                else
                    ClearError(RegisterErrors.InvalidDate, Properties.BirthDate);

                if ((DateTime.Now.Year - value.Year) < 12)
                    RaiseError(RegisterErrors.AgeMinimum, Properties.BirthDate, "L'age minimal pour pouvoir accéder à un espace client est de 12 ans.");
                else
                    ClearError(RegisterErrors.AgeMinimum, Properties.BirthDate);

                SetProperty(ref m_BirthDate, value);
            }
        }

        private string m_Street;

        public string Street
        {
            get { return m_Street; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                    RaiseError(RegisterErrors.InvalidField, Properties.Street, "Veuillez fournir la rue et le numéro");
                else
                    ClearError(RegisterErrors.InvalidField, Properties.Street);

                SetProperty(ref m_Street, value.Trim().ToLower());
            }
        }

        private string m_Appartment;

        public string Appartment
        {
            get { return m_Appartment; }
            set
            {
                if (string.IsNullOrEmpty(value.Trim()))
                    value = string.Empty;
                SetProperty(ref m_Appartment, value.Trim().ToLower());
            }
        }


        private string m_ZipCode;

        public string ZipCode
        {
            get { return m_ZipCode; }
            set
            {
                if (string.IsNullOrWhiteSpace(value.Trim()))
                    RaiseError(RegisterErrors.InvalidField, Properties.ZipCode, "Veuillez fournir le code postal!");
                else
                    ClearError(RegisterErrors.InvalidField, Properties.ZipCode);

                if (value.Length != 4)
                    RaiseError(RegisterErrors.InvalidZipCode, Properties.ZipCode, "Le format du code postal est incorrect");
                else
                    ClearError(RegisterErrors.InvalidZipCode, Properties.ZipCode);

                SetProperty(ref m_ZipCode, value.Trim().ToLower());
            }
        }

        private string m_City;

        public string City
        {
            get { return m_City; }
            set
            {
                if (string.IsNullOrWhiteSpace(value.Trim()))
                    RaiseError(RegisterErrors.InvalidField, Properties.City, "Veuillez fournir la ville!");
                else
                    ClearError(RegisterErrors.InvalidField, Properties.City);

                SetProperty(ref m_City, value.Trim().ToLower());
            }
        }

        private string m_PhoneNumber;

        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value.Trim()))
                    RaiseError(RegisterErrors.InvalidField, Properties.PhoneNumber, "Veuillez fournir le numéro de téléphone!");
                else
                    ClearError(RegisterErrors.InvalidField, Properties.PhoneNumber);

                if (value.Trim().Length != 10)
                    RaiseError(RegisterErrors.InvalidPhone, Properties.PhoneNumber, "Le numéro de téléphone est invalide!");
                else
                    ClearError(RegisterErrors.InvalidPhone, Properties.PhoneNumber);

                SetProperty(ref m_PhoneNumber, value.Trim().ToLower());
            }
        }
        #endregion

        public RegisterViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            GendersList = new(GendersData.GetGenders());

            RegisterCommand = new(async () => await Register());
        }

        /// <summary>
        /// Enregistre le nouvel utilisateur dans la base de donnée en entrant
        /// ses informations et son adresse. Une fois l'enregistrement terminé
        /// l'utilisateur est renvoyé vers la page précédente
        /// </summary>
        public async Task Register()
        {
            if (CanRegister())
            {
                AddressForCreationRecord address = new(Street, Appartment, ZipCode, City);
                var addressToCreate = m_Mapper.Map<AddressEntity>(address);
                if (addressToCreate == null)
                {
                    ErrorClaim.RaiseError("Address couldn't be mapped", address, addressToCreate);
                    return;
                }
                UserForCreationRecord user = new(Email, Password, FirstName, LastName, PhoneNumber, Gender, BirthDate, address);
                var userToCreate = m_Mapper.Map<UserEntity>(user);
                if (user == null)
                {
                    ErrorClaim.RaiseError("User couldn't be mapped", user, userToCreate);
                    return;
                }
                m_Repository.AddUser(userToCreate);
                await m_Repository.SaveAsync();
                GoBack();
            }
        }

        /// <summary>
        /// Crée une erreur dans la liste ErrorsList
        /// </summary>
        /// <param name="errorType">Type d'erreur</param>
        /// <param name="property">La propriété sur laquelle l'erreur est produite</param>
        /// <param name="errorMessage">Le message a afficher</param>
        public void RaiseError(RegisterErrors errorType, Properties property, string errorMessage)
        {
            var error = ErrorsList.FirstOrDefault(x => x.Property.Equals(GetPropertyInfo(property)));
            if (error == null)
            {
                ErrorsList.Add(new(errorType, GetPropertyInfo(property), errorMessage));
            }
            else
            {
                if (!error.ErrorMessage.Equals(errorMessage) && !error.ErrorType.Equals(errorType))
                {
                    int i = ErrorsList.IndexOf(error);
                    ErrorRecord newError = new(errorType, GetPropertyInfo(property), errorMessage);
                    ErrorsList.RemoveAt(i);
                    ErrorsList.Insert(i, newError);
                }
            }
        }

        /// <summary>
        /// Supprime l'erreur défini par le type d'erreur et la propriété sur laquelle
        /// l'erreur est créee de la liste des erreurs ErrorsList
        /// </summary>
        /// <param name="errorType">Type d'erreur</param>
        /// <param name="property">Propriété sur laquelle l'erreur est produite</param>
        public void ClearError(RegisterErrors errorType, Properties property)
        {
            var error = ErrorsList.FirstOrDefault(x => x.ErrorType.Equals(errorType) && x.Property.Equals(GetPropertyInfo(property)));
            if (error != null)
            {
                ErrorsList.Remove(error);
            }
        }

        /// <summary>
        /// Vérifie bien si tous les champs requis à l'enregistrement d'un nouvel utilisateur
        /// ne sont pas vide.
        /// </summary>
        /// <returns>
        /// true Si tous les champs contienent au moins 1 caractère. false Dans le cas contraire
        /// </returns>
        public bool AllFieldsAreFull()
        {
            return !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(EmailConfirmation) &&
                !string.IsNullOrEmpty(Password) &&
                !string.IsNullOrEmpty(PasswordConfirmation) &&
                !string.IsNullOrEmpty(FirstName) &&
                !string.IsNullOrEmpty(LastName) &&
                Gender != null &&
                !string.IsNullOrEmpty(Gender.Name) &&
                !string.IsNullOrEmpty(Street) &&
                !string.IsNullOrEmpty(ZipCode) &&
                !string.IsNullOrEmpty(City) &&
                !string.IsNullOrEmpty(PhoneNumber);
        }

        /// <summary>
        /// Vérifie grâce aux expressions régulières si le mot de passes contient bien une lettre
        /// minuscule, une lettre majuscule, un chiffre et un caractère spécial.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>
        /// true Si le mot de passe répond bien aux conditions énumérées dans le résumé. false dans
        /// le cas contraire
        /// </returns>
        public bool PasswordIsValid(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,25}$");
        }

        /// <summary>
        /// Vérification supplémentaire sur le mail pour bien vérifier que le champs n'est pas 
        /// vide et que l'email est valide.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>
        /// true Si le mail est valide. false Dans le cas contraire
        /// </returns>
        public bool EmailIsValid(string email)
        {
            return !string.IsNullOrEmpty(email) || EmailHelper.IsValidEmail(email);
        }

        /// <summary>
        /// Vérifie si toutes les conditions à l'enregistrement d'un nouveau compte sont correctes 
        /// (Emails valides et identiques, mot de passes valides et identiques, tous les champs 
        /// (excepté l'appartement) sont remplis, l'utilisateur est agé de 12 ans ou plus)
        /// </summary>
        /// <returns>
        /// true Si les conditions énumérées sont remplis. false dans le case contraire
        /// </returns>
        public bool CanRegister()
        {
            return EmailIsValid(Email) && EmailIsValid(EmailConfirmation) && Email.Equals(EmailConfirmation) &&
                PasswordIsValid(Password) && PasswordIsValid(PasswordConfirmation) && Password.Equals(PasswordConfirmation) &&
                AllFieldsAreFull() &&
                ((DateTime.Now.Year - BirthDate.Year) > 12);
        }

        /// <summary>
        /// Récupère la propriété de la classe grâce à la propriété déclaré
        /// dans l'enum Properties
        /// </summary>
        /// <param name="property">Element de l'enum Properties représentant
        /// une propriété de la class</param>
        /// <returns>
        /// La propriété si elle est trouvée. null Dans le cas contraire
        /// </returns>
        public PropertyInfo GetPropertyInfo(Properties property) => GetType().GetProperty(property.ToString());

        /// <summary>
        /// Fait naviguer le programme à la page précédente
        /// </summary>
        public void GoBack()
        {
            if (m_Navigation.Journal.CanGoBack)
            {
                m_Navigation.Journal.GoBack();
            }
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
        }

        public bool PersistInHistory()
        {
            return false;
        }
    }
}
