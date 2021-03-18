using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.DefaultData;
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
    public class ProfileInformationsViewModel : BindableBase, INavigationAware, IJournalAware
    {
        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;

        private IRegionNavigationService m_Navigation;

        private UserCurrentSessionRecord m_CurrectSession;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand ModifyCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        private ObservableCollection<GenderRecord> m_GendersCollection;

        public ObservableCollection<GenderRecord> GendersCollection
        {
            get { return m_GendersCollection; }
            set { SetProperty(ref m_GendersCollection, value); }
        }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private UserForUpdateModel m_User;

        public UserForUpdateModel User
        {
            get { return m_User; }
            set { SetProperty(ref m_User, value); }
        }


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
            set { SetProperty(ref m_FirstName, value); }
        }

        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set { SetProperty(ref m_LastName, value); }
        }

        private string m_PhoneNumber;

        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set { SetProperty(ref m_PhoneNumber, value); }
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
            set { SetProperty(ref m_BirthDate, value); }
        }
        #endregion

        public ProfileInformationsViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            GendersCollection = new(GendersData.GetGenders());

            ModifyCommand = new(async () => await Modify());
        }

        /// <summary>
        /// Charge les données initiales à afficher
        /// </summary>
        public async Task LoadData()
        {
            User = m_Mapper.Map<UserForUpdateModel>(await m_Repository.GetUserAsync(m_CurrectSession.Id));
            Gender = GendersCollection.FirstOrDefault(x => x.Name.Equals(User.Gender.Name));
        }

        public async Task Modify()
        {
            User.Gender = new GenderRecord(Gender.Name);
            var userToUpdate = await m_Repository.GetUserAsync(User.Id);
            m_Mapper.Map(User, userToUpdate);
            await m_Repository.SaveAsync();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (m_CurrectSession == null) m_CurrectSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(NavParameters.CurrentSessionParam);
            Task.Run(LoadData);
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
            return false;
        }

    }
}
