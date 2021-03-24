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
    public class AdminBookViewModel : BindableBase, INavigationAware
    {
        public enum AdminBookViews
        {
            AdminAddBookView,
            AdminModifyBookView
        }

        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;
        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "AdminRegion";

        private IRegionNavigationService m_Navigation;
        private UserCurrentSessionRecord m_CurrentSession;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand DeleteSelectedBooksCommand { get; set; }
        public DelegateCommand<int> NavigateToModifyBookViewCommand { get; set; }
        public DelegateCommand NavigateToAdminBookAddViewCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        public ObservableCollection<BookAdminMiniatureModel> Books { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

        private string m_Search;

        public string SearchKeyWord
        {
            get { return m_Search; }
            set { SetProperty(ref m_Search, value); }
        }


        public AdminBookViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            SearchCommand = new(async () => await Search());
            DeleteSelectedBooksCommand = new(DeleteSelectedBooks);
            NavigateToModifyBookViewCommand = new DelegateCommand<int>(NavigateToAdminModifyBookView);
            NavigateToAdminBookAddViewCommand = new(NavigateToAdminAddBookView);
        }

        public async Task Search()
        {
            if (!string.IsNullOrEmpty(SearchKeyWord))
            {
                Books = new(m_Mapper.Map<IEnumerable<BookAdminMiniatureModel>>(await m_Repository.GetBooks(SearchKeyWord)));
            }
        }

        public void DeleteSelectedBooks()
        {
            var result = Books.Where(x => x.Selected == true);
            m_Repository.DeleteBooks(m_Mapper.Map<IEnumerable<BookEntity>>(result));
        }

        public void NavigateToAdminModifyBookView(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("BookId", id);
            Navigate(AdminBookViews.AdminModifyBookView, parameters);
        }

        public void NavigateToAdminAddBookView()
        {
            Navigate(AdminBookViews.AdminAddBookView);
        }

        public void Navigate(AdminBookViews view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { NavParameters.CurrentSessionParam, m_CurrentSession }
            };

            if (navigationParams != null)
            {
                foreach (var navigationParam in navigationParams)
                {
                    navigationParameters.Add(navigationParam.Key, navigationParam.Value);
                }
            }
            m_Region.RequestNavigate(m_RegionName, new Uri(view.ToString(), UriKind.Relative), navigationParameters);
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
            if (m_CurrentSession == null) m_CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(NavParameters.CurrentSessionParam);
            if (m_CurrentSession == null)
            {
                m_Navigation.Journal.GoBack();
                m_Navigation.Journal.Clear();
            }
        }
    }
}
