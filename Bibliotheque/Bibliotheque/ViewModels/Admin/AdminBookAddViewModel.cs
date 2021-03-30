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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class AdminBookAddViewModel : BaseViewModel
    {
        private IRegionManager m_Region;
        private BookModel Book;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand LoadCommand { get; set; }
        public DelegateCommand AddBookCommand { get; set; }
        public DelegateCommand AddGenreToBookCommand { get; set; }
        public DelegateCommand RemoveGenreFromBookCommand { get; set; }
        public DelegateCommand AddImageCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/
        #region Collection de la vue
        private ObservableCollection<string> m_Authors;

        public ObservableCollection<string> Authors
        {
            get => m_Authors;
            set => SetProperty(ref m_Authors, value);
        }

        private ObservableCollection<string> m_Editors;

        public ObservableCollection<string> Editors
        {
            get => m_Editors;
            set => SetProperty(ref m_Editors, value);
        }

        private ObservableCollection<string> m_Formats;

        public ObservableCollection<string> Formats
        {
            get => m_Formats;
            set => SetProperty(ref m_Formats, value);
        }

        private ObservableCollection<CategoryModel> m_Categories;

        public ObservableCollection<CategoryModel> Categories
        {
            get => m_Categories;
            set => SetProperty(ref m_Categories, value);
        }

        private ObservableCollection<GenreModel> m_Genres;

        public ObservableCollection<GenreModel> Genres
        {
            get => m_Genres;
            set => SetProperty(ref m_Genres, value);
        }

        private ObservableCollection<GenreModel> m_BookGenres;

        public ObservableCollection<GenreModel> BookGenres
        {
            get => m_BookGenres;
            set => SetProperty(ref m_BookGenres, value);
        }
        #endregion
        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés de la vue
        private string m_Title;

        public string Title
        {
            get { return m_Title; }
            set
            {
                var result = Book.DefineTitle(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Title, value);
            }
        }

        private string m_Author;

        public string Author
        {
            get { return m_Author; }
            set
            {
                var result = Book.DefineAuthor(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Author, value);
            }
        }

        private string m_Summary;

        public string Summary
        {
            get { return m_Summary; }
            set
            {
                var result = Book.DefineSummary(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Summary, value);
            }
        }

        private DateTime m_ReleaseDate = DateTime.MinValue;

        public DateTime ReleaseDate
        {
            get { return m_ReleaseDate; }
            set
            {
                var result = Book.DefineReleaseDate(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_ReleaseDate, value);
            }
        }

        private string m_Editor;

        public string Editor
        {
            get { return m_Editor; }
            set
            {
                var result = Book.DefineEditor(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Editor, value);
            }
        }

        private string m_Format;

        public string Format
        {
            get { return m_Format; }
            set
            {
                var result = Book.DefineFormat(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Format, value);
            }
        }

        private int m_Pages;

        public int Pages
        {
            get { return m_Pages; }
            set
            {
                var result = Book.DefinePages(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Pages, value);
            }
        }

        private string m_EAN;

        public string EAN
        {
            get { return m_EAN; }
            set
            {
                var result = Book.DefineEAN(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_EAN, value);
            }
        }

        private string m_ISBN;

        public string ISBN
        {
            get { return m_ISBN; }
            set
            {
                var result = Book.DefineISBN(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_ISBN, value);
            }
        }

        private CategoryModel m_Category;

        public CategoryModel Category
        {
            get { return m_Category; }
            set
            {
                var result = Book.DefineCategory(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_Category, value);
            }
        }

        private string m_NewCategory;

        public string NewCategory
        {
            get { return m_NewCategory; }
            set
            {
                if (Category is null || !Categories.Any(x => x.Name.Equals(value)))
                {
                    Category = new CategoryModel();
                    var result = Category.DefineName(value);
                    CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                }
                SetProperty(ref m_NewCategory, value);
            }
        }

        private GenreModel m_Genre;

        public GenreModel Genre
        {
            get { return m_Genre; }
            set { SetProperty(ref m_Genre, value); }
        }

        private string m_NewGenre;

        public string NewGenre
        {
            get { return m_NewGenre; }
            set
            {
                if (Genre is null || Genres.Where(x => x.Name is not null && x.Name.Equals(value)).Count() == 0)
                {
                    Genre = new GenreModel();
                    var result = Genre.DefineName(value);
                    CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                }
                SetProperty(ref m_NewGenre, value);
            }
        }

        private GenreModel m_BookGenre;

        public GenreModel BookGenre
        {
            get { return m_BookGenre; }
            set { SetProperty(ref m_BookGenre, value); }
        }

        private string m_ImagePath;

        public string ImagePath
        {
            get { return m_ImagePath; }
            set
            {
                var result = Book.DefinePrefacePath(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_ImagePath, value);
            }
        }
        #endregion

        public AdminBookAddViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            // Chargement des commandes
            LoadCommand = new(async () => await Load());
            AddBookCommand = new(async () => await AddBook());
            AddGenreToBookCommand = new(AddGenreToBook);
            RemoveGenreFromBookCommand = new(RemoveGenreFromBook);
        }

        public async Task Load()
        {
            var books = await m_Repository.GetBooksAsync();
            Authors = new(books.Select(p => p.Author));
            Editors = new(books.Select(p => p.Editor));
            Formats = new(books.Select(p => p.Format));
            Categories = new(m_Mapper.Map<IEnumerable<CategoryModel>>(await m_Repository.GetCategoriesAsync()));
            Genres = new(m_Mapper.Map<IEnumerable<GenreModel>>(await m_Repository.GetGenresAsync()));
            BookGenres = new();
            Errors = new();
            Book = new();
        }

        public async Task AddBook()
        {
            bool genresValid = (BookGenres.Count >= 1);
            CheckError("Genres", "Un livre doit avoir au moins un genre !", genresValid);
            bool bookValid = Book.IsValid();
            CheckError("Livre", "Un ou des champs du livres sont invalides !", bookValid);
            if (genresValid == true && bookValid == true)
            {
                string newPath = $"../../../Images/{Title}-{Editor}-{Format}.jpg";
                File.Copy(ImagePath, newPath, true);
                var bookToAdd = m_Mapper.Map<BookEntity>(Book);
                bookToAdd.CategoryId = Category.Id;
                bookToAdd.Preface = newPath;
                bookToAdd.Genres = new List<BookGenreEntity>();
                foreach (var genre in BookGenres)
                {
                    var genreToAdd = await m_Repository.GetGenreAsync(genre.Id);
                    if (genreToAdd is null) genreToAdd = m_Mapper.Map<GenreEntity>(genre);
                    bookToAdd.Genres.Add(new BookGenreEntity()
                    {
                        Book = bookToAdd,
                        Genre = genreToAdd,
                        GenreId = genreToAdd.Id,
                    });
                }
                m_Repository.AddBook(bookToAdd);
                await m_Repository.SaveAsync();
                GoBack();
            }
        }

        public void AddGenreToBook()
        {
            if (Genre.IsValid() && !BookGenres.Contains(Genre))
            {
                BookGenres.Add(Genre);
                Genres.Remove(Genre);
            }
        }

        public void RemoveGenreFromBook()
        {
            if (BookGenre is not null)
            {
                Genres.Add(BookGenre);
                BookGenres.Remove(BookGenre);
            }
        }

        public void NavigateBack()
        {
            Navigate(ViewsEnum.AdminBooksView);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            if (m_Region is null) m_Region = navigationContext.Parameters.GetValue<IRegionManager>("AdminRegion");
            if (CurrentSession is null)
            {
                GoBack();
                m_NavigationService.Journal.Clear();
            }
        }
    }
}
