using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ProfileInformationsViewModel : BaseViewModel
    {
        public UserModel User { get; set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand ModifyCommand { get; set; }

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
            set { SetProperty(ref m_Email, value); }
        }

        private string m_FirstName;

        public string FirstName
        {
            get { return m_FirstName; }
            set
            {
                var result = User.DefineFirstName(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_FirstName, value);
            }
        }

        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set
            {
                var result = User.DefineLastName(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_LastName, value);
            }
        }

        private string m_PhoneNumber;

        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set
            {
                var result = User.DefinePhoneNumber(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_PhoneNumber, value);
            }
        }


        private string m_Gender;

        public string Gender
        {
            get { return m_Gender; }
            set
            {
                var result = User.DefineGender(value);
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
                var result = User.DefineBirthDate(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_BirthDate, value);
            }
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public ProfileInformationsViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            Genders = new(GendersData.GetGenders());

            LoadCommand = new(async () => await Load());
            ModifyCommand = new(async () => await Modify());
        }

        /// <summary>
        /// Charge les données initiales à afficher
        /// </summary>
        public async Task Load()
        {
            User = m_Mapper.Map<UserModel>(await m_Repository.GetUserAsync(CurrentSession.Id));
            Gender = Genders.FirstOrDefault(x => x.Equals(User.Gender));

            Email = User.Email;
            FirstName = User.FirstName;
            LastName = User.LastName;
            PhoneNumber = User.PhoneNumber;
            Gender = User.Gender;
            BirthDate = User.BirthDate;
        }

        public async Task Modify()
        {
            bool valid = User.IsValid();
            CheckError("Valid", "Un ou des champs ne sont pas valides !", valid);
            if (valid == true)
            {
                var userToUpdate = await m_Repository.GetUserAsync(User.Id);
                m_Mapper.Map(User, userToUpdate);
                await m_Repository.SaveAsync();
            }
        }
    }
}
