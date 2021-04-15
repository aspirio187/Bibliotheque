using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class BooksViewModel : BaseViewModel
    {
        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand<int?> NavigateToBookDetailsCommand { get; set; }
        public DelegateCommand ClearGenreCommand { get; set; }
        public DelegateCommand ClearCategoryCommmand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        private ObservableCollection<BookMiniatureModel> m_Books;

        public ObservableCollection<BookMiniatureModel> Books
        {
            get => m_Books;
            set
            {
                SetProperty(ref m_Books, value);
            }
        }

        private ObservableCollection<GenreModel> m_Genres;

        public ObservableCollection<GenreModel> Genres
        {
            get { return m_Genres; }
            set { SetProperty(ref m_Genres, value); }
        }

        private ObservableCollection<CategoryModel> m_Categories;

        public ObservableCollection<CategoryModel> Categories
        {
            get { return m_Categories; }
            set { SetProperty(ref m_Categories, value); }
        }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private string m_SearchText;

        public string SearchText
        {
            get { return m_SearchText; }
            set { SetProperty(ref m_SearchText, value); }
        }

        private GenreModel m_Genre;

        public GenreModel Genre
        {
            get { return m_Genre; }
            set
            {
                SetProperty(ref m_Genre, value);
                Task.Run(FilterBooks);
            }
        }

        private CategoryModel m_Category;

        public CategoryModel Category
        {
            get { return m_Category; }
            set
            {
                SetProperty(ref m_Category, value);
                Task.Run(FilterBooks).Wait();
            }
        }
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public BooksViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            LoadCommand = new(async () => await LoadAsync());
            SearchCommand = new(async () => await FilterBooks());
            NavigateToBookDetailsCommand = new DelegateCommand<int?>(NavigateToBookDetails);
            ClearGenreCommand = new(ClearGenre);
            ClearCategoryCommmand = new(ClearCategory);
        }

        public void NavigateToBookDetails(int? bookId)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("BookId", bookId);
            Navigate(ViewsEnum.BookDetailsView, parameters);
        }

        public void ClearGenre()
        {
            Genre = null;
            Task.Run(FilterBooks);
        }

        public void ClearCategory()
        {
            Category = null;
            Task.Run(FilterBooks);
        }

        public async Task FilterBooks()
        {
            var books = await m_Repository.GetBooksAsync();
            if (Genre is not null)
            {
                var genre = await m_Repository.GetGenreAsync(Genre.Id);
                if (genre is not null)
                {
                    books = books.Where(b => b.BookGenres.Any(bg => bg.Genre == genre));
                }

            }
            if (Category is not null)
            {
                books = books.Where(b => b.CategoryId == Category.Id);
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                books.Where(x => x.Title.ToLower().Contains(SearchText));
            }
            Books = new(m_Mapper.Map<IEnumerable<BookMiniatureModel>>(books));
        }

        public override async Task LoadAsync()
        {
            await LoadBooksAsync();
            Genres = new(m_Mapper.Map<IEnumerable<GenreModel>>(await m_Repository.GetGenresAsync()));
            Categories = new(m_Mapper.Map<IEnumerable<CategoryModel>>(await m_Repository.GetCategoriesAsync()));
        }

        public async Task LoadBooksAsync()
        {
            Books = new(m_Mapper.Map<IEnumerable<BookMiniatureModel>>(await m_Repository.GetBooksAsync()));
        }
    }
}
