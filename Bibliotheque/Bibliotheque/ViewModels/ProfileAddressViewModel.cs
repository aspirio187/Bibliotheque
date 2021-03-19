using AutoMapper;
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
using System.Threading;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ProfileAddressViewModel : BindableBase, INavigationAware, IJournalAware
    {
        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;

        private UserCurrentSessionRecord m_CurrentSession;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand ModifyCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private AddressForUpdateModel m_Address;

        public AddressForUpdateModel Address
        {
            get { return m_Address; }
            set { SetProperty(ref m_Address, value); }
        }
        #endregion

        public ProfileAddressViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            ModifyCommand = new(async () => await Modify());
        }

        public async Task Load()
        {
            Address = m_Mapper.Map<AddressForUpdateModel>(await m_Repository.GetUserAddress(m_CurrentSession.Id));
        }

        public async Task Modify()
        {
            if (Address.FieldsAreValid())
            {
                var addressToUpdate = await m_Repository.GetAddress(Address.Id);
                m_Mapper.Map(Address, addressToUpdate);
                await m_Repository.SaveAsync();
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
            if (m_CurrentSession == null) m_CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(NavParameters.CurrentSessionParam);
            Task.Run(Load);
        }

        public bool PersistInHistory()
        {
            return false;
        }
    }
}
