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
    public class AdminBooksViewModel : BaseViewModel
    {
        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "AdminRegion";

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand DeleteSelectedBooksCommand { get; set; }
        public DelegateCommand<int?> NavigateToAdminModifyBookViewCommand { get; set; }
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


        public AdminBooksViewModel(ILibraryRepository repository, IMapper mapper, IRegionManager region) :
            base(repository, mapper)
        {
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));

            LoadCommand = new(async () => await LoadAsync());
            SearchCommand = new(async () => await Search());
            DeleteSelectedBooksCommand = new(async () => await DeleteSelectedBooks());
            NavigateToAdminModifyBookViewCommand = new DelegateCommand<int?>(NavigateToAdminModifyBookView);
            NavigateToAdminBookAddViewCommand = new(NavigateToAdminAddBookView);
        }

        public override async Task LoadAsync()
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

        public void NavigateToAdminModifyBookView(int? id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("BookId", id);
            Navigate(ViewsEnum.AdminBookModifyView, parameters);
        }

        public void NavigateToAdminAddBookView()
        {
            Navigate(ViewsEnum.AdminBookAddView);
        }

        public override void Navigate(ViewsEnum view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { GlobalInfos.CurrentSession, CurrentSession },
                { GlobalInfos.NavigationService, m_NavigationService }
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

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (CurrentSession == null)
            {
                m_NavigationService.Journal.GoBack();
                m_NavigationService.Journal.Clear();
            }
        }
    }
}
