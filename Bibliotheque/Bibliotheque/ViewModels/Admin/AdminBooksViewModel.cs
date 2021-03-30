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
    public class AdminBooksViewModel : BindableBase, INavigationAware, IJournalAware
    {
        public enum AdminBookViews
        {
            AdminBookAddView,
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

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand DeleteSelectedBooksCommand { get; set; }
        public DelegateCommand<object> NavigateToModifyBookViewCommand { get; set; }
        public DelegateCommand NavigateToAdminBookAddViewCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        private ObservableCollection<BookAdminMiniatureModel> m_Books;

        public ObservableCollection<BookAdminMiniatureModel> Books
        {
            get => m_Books;
            set
            {
                SetProperty(ref m_Books, value);
            }
        }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

        private string m_Search;

        public string SearchKeyWord
        {
            get { return m_Search; }
            set { SetProperty(ref m_Search, value); }
        }


        public AdminBooksViewModel(ILibraryRepository repository, IMapper mapper, IRegionManager region)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));

            LoadCommand = new(async () => await Load());
            SearchCommand = new(async () => await Search());
            DeleteSelectedBooksCommand = new(async () => await DeleteSelectedBooks());
            NavigateToModifyBookViewCommand = new DelegateCommand<object>(NavigateToAdminModifyBookView);
            NavigateToAdminBookAddViewCommand = new(NavigateToAdminAddBookView);
        }

        public async Task Load()
        {
            var books = await m_Repository.GetBooksAsync();
            Books = new(m_Mapper.Map<IEnumerable<BookAdminMiniatureModel>>(books));
        }

        public async Task Search()
        {
            if (!string.IsNullOrEmpty(SearchKeyWord))
            {
                Books = new(m_Mapper.Map<IEnumerable<BookAdminMiniatureModel>>(await m_Repository.GetBooks(SearchKeyWord)));
            }
            else
            {
                Books = new(m_Mapper.Map<IEnumerable<BookAdminMiniatureModel>>(await m_Repository.GetBooksAsync()));
            }
        }

        public async Task DeleteSelectedBooks()
        {
            var result = Books.Where(x => x.Selected == true);
            List<BookEntity> books = new();
            foreach (var book in result)
            {
                books.Add(await m_Repository.GetBookAsync(book.Id));
            }
            m_Repository.DeleteBooks(books);
            await m_Repository.SaveAsync();
            LoadCommand.Execute();
        }

        public void NavigateToAdminModifyBookView(object id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("BookId", (int)id);
            Navigate(AdminBookViews.AdminModifyBookView, parameters);
        }

        public void NavigateToAdminAddBookView()
        {
            Navigate(AdminBookViews.AdminBookAddView);
        }

        public void Navigate(AdminBookViews view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { GlobalInfos.CurrentSession, m_CurrentSession }
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
            if (m_Navigation == null) m_Navigation = navigationContext.Parameters.GetValue<IRegionNavigationService>(GlobalInfos.NavigationService);
            if (m_CurrentSession == null) m_CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(GlobalInfos.CurrentSession);
            if (m_CurrentSession == null)
            {
                m_Navigation.Journal.GoBack();
                m_Navigation.Journal.Clear();
            }
        }

        public bool PersistInHistory()
        {
            return false;
        }
    }
}
