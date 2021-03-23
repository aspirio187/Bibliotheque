using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class HomeViewModel : BindableBase, INavigationAware
    {
        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;

        private IRegionNavigationService m_Navigation;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand NavigateToRegisterCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        public ObservableCollection<BookMiniatureModel> LastAddedBooks { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        private string m_ImagePath;
        public string ImagePath { get => m_ImagePath; set => SetProperty(ref m_ImagePath, value); }

        public HomeViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            string imagePath = "../../../Images/Le throne de fer.jpg";
            string fullPath = Path.GetFullPath(imagePath);
            ImagePath = fullPath;

            LastAddedBooks = new(m_Mapper.Map<IEnumerable<BookMiniatureModel>>(m_Repository.GetLastBooks()));
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
    }
}
