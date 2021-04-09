using AutoMapper;
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
using System.Threading;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ProfileAddressViewModel : BaseViewModel
    {
        public AddressModel Address { get; set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand ModifyCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private string m_Street;

        public string Street
        {
            get { return m_Street; }
            set
            {
                var result = Address.DefineStreet(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Street, value);
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
                SetProperty(ref m_Appartment, value);
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
                SetProperty(ref m_ZipCode, value);
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
                SetProperty(ref m_City, value);
            }
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public ProfileAddressViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            LoadCommand = new(async () => await LoadAsync());
            ModifyCommand = new(async () => await Modify());
        }

        public override async Task LoadAsync()
        {
            Address = m_Mapper.Map<AddressModel>(await m_Repository.GetUserAddress(CurrentSession.Id));
            Street = Address.Street;
            Appartment = Address.Appartment;
            ZipCode = Address.ZipCode;
            City = Address.City;
        }

        public async Task Modify()
        {
            bool valid = Address.IsValid();
            CheckError("Adresse", "Un ou des champs de l'adresse sont invalides !", valid);
            if (valid == true)
            {
                var addressToUpdate = await m_Repository.GetAddress(Address.Id);
                m_Mapper.Map(Address, addressToUpdate);
                await m_Repository.SaveAsync();
            }
        }
    }
}
