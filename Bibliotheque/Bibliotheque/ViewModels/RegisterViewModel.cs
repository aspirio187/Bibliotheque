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
    public class RegisterViewModel : BaseViewModel
    {
        public UserRegistrationModel UserRegistration { get; }
        public AddressModel Address { get; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand RegisterCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        private ObservableCollection<string> m_Genders;

        public ObservableCollection<string> Genders
        {
            get { return m_Genders; }
            set { SetProperty(ref m_Genders, value); }
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
                var result = UserRegistration.DefineEmail(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Email, value);
            }
        }

        private string m_EmailConfirmation;

        public string EmailConfirmation
        {
            get { return m_EmailConfirmation; }
            set
            {
                var result = UserRegistration.DefineEmailConfirmation(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_EmailConfirmation, value);
            }
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set
            {
                var result = UserRegistration.DefinePassword(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Password, value);
            }
        }

        private string m_PasswordConfirmation;

        public string PasswordConfirmation
        {
            get { return m_PasswordConfirmation; }
            set
            {
                var result = UserRegistration.DefinePasswordConfirmation(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_PasswordConfirmation, value);
            }
        }

        private string m_FirstName;

        public string FirstName
        {
            get { return m_FirstName; }
            set
            {
                var result = UserRegistration.DefineFirstName(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_FirstName, value.Trim().ToLower().FirstCharToUpper());
            }
        }

        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set
            {
                var result = UserRegistration.DefineLastName(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_LastName, value.Trim().ToLower().FirstCharToUpper());
            }
        }

        private string m_Gender;

        public string Gender
        {
            get { return m_Gender; }
            set
            {
                var result = UserRegistration.DefineGender(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Gender, value);
            }
        }

        private DateTime m_BirthDate;

        public DateTime BirthDate
        {
            get { return m_BirthDate; }
            set
            {
                var result = UserRegistration.DefineBirthDate(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_BirthDate, value);
            }
        }

        private string m_Street;

        public string Street
        {
            get { return m_Street; }
            set
            {
                var result = Address.DefineStreet(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Street, value.Trim().ToLower());
            }
        }

        private string m_Appartment;

        public string Appartment
        {
            get { return m_Appartment; }
            set
            {
                var result = Address.DefineAppartment(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Appartment, value.Trim().ToLower());
            }
        }


        private string m_ZipCode;

        public string ZipCode
        {
            get { return m_ZipCode; }
            set
            {
                var result = Address.DefineZipCode(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_ZipCode, value.Trim().ToLower());
            }
        }

        private string m_City;

        public string City
        {
            get { return m_City; }
            set
            {
                var result = Address.DefineCity(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_City, value.Trim().ToLower());
            }
        }

        private string m_PhoneNumber;

        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set
            {
                var result = UserRegistration.DefinePhoneNumber(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_PhoneNumber, value.Trim().ToLower());
            }
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository">ILibraryRepository</param>
        /// <param name="mapper">IMapper</param>
        /// <exception cref="ArgumentNullException">
        /// Si repository ou mapper sont null
        /// </exception>
        public RegisterViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            UserRegistration = new();
            Address = new();

            Genders = new(GendersData.GetGenders());

            RegisterCommand = new(async () => await Register());
        }

        /// <summary>
        /// Enregistre le nouvel utilisateur dans la base de donnée en entrant
        /// ses informations et son adresse. Une fois l'enregistrement terminé
        /// l'utilisateur est renvoyé vers la page précédente
        /// </summary>
        public async Task Register()
        {
            var result = UserRegistration.DefineAddress(Address);
            CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
            if (UserRegistration.IsValid())
            {
                var userToCreate = m_Mapper.Map<UserEntity>(UserRegistration);
                if (userToCreate is null)
                {
                    Debug.WriteLine($"User couldn't be mapped : {userToCreate}");
                    return;
                }
                //var addressToCreate = m_Mapper.Map<AddressEntity>(Address);
                //if (addressToCreate is null)
                //{
                //    Debug.WriteLine($"Address couldn't be mapped : {addressToCreate}");
                //    return;
                //}
                m_Repository.AddUser(userToCreate);
                await m_Repository.SaveAsync();
                GoBack();
            }
        }
    }
}
