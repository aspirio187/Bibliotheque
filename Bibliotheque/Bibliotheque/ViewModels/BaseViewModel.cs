using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bibliotheque.UI.ViewModels.ProfileViewModel;

namespace Bibliotheque.UI.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INavigationAware, IJournalAware
    {
        /********************************************************************/
        /*********************** Propriétés readonly ************************/
        /********************************************************************/

        protected readonly ILibraryRepository m_Repository;
        protected readonly IMapper m_Mapper;

        /********************************************************************/
        /********************** Propriétés transmises ***********************/
        /********************************************************************/

        protected IRegionNavigationService m_NavigationService;

        public UserCurrentSessionRecord CurrentSession { get; protected set; }

        /********************************************************************/
        /****************** Collections relatives à la vue ******************/
        /********************************************************************/

        private ObservableCollection<ErrorModel> m_Errors;

        public ObservableCollection<ErrorModel> Errors
        {
            get { return m_Errors; }
            set { SetProperty(ref m_Errors, value); }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository">ILibraryRepository</param>
        /// <param name="mapper">IMapper</param>
        /// <exception cref="ArgumentNullException">
        /// Si repository ou mapper en paramètre est null
        /// </exception>
        public BaseViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Navigue vers la page de profile définie par l'élément de l'enum ProfileViews qui représente
        /// le nom d'une ProfileView
        /// </summary>
        /// <param name="view">
        /// Vue désirée
        /// </param>
        /// <param name="navigationParams">
        /// Dictionnaire de paramètre passable à la vue
        /// </param>
        public void Navigate(ViewsEnum view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { GlobalInfos.CurrentSession, CurrentSession }
            };

            if (navigationParams is not null)
            {
                foreach (var navigationParam in navigationParams)
                {
                    navigationParameters.Add(navigationParam.Key, navigationParam.Value);
                }
            }
            m_NavigationService.RequestNavigate(new Uri(view.ToString(), UriKind.Relative), navigationParameters);
        }

        /// <summary>
        /// Fonction de retour en arrière dans la pile de navigation
        /// </summary>
        protected void GoBack()
        {
            if (m_NavigationService.Journal.CanGoBack)
            {
                m_NavigationService.Journal.GoBack());
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Rien pour le moment
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (m_NavigationService is null) m_NavigationService = navigationContext.Parameters.GetValue<IRegionNavigationService>(GlobalInfos.NavigationService);
            if (CurrentSession is null) CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(GlobalInfos.CurrentSession);
        }

        public bool PersistInHistory()
        {
            return true;
        }
    }
}
