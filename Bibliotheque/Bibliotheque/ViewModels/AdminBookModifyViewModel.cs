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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class AdminBookModifyViewModel : BaseViewModel
    {
        public BookModel Book { get; set; }
        public int BookId { get; set; }
        public List<BookStateModel> DeletedStates { get; set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand AddGenreToBookCommand { get; set; }
        public DelegateCommand RemoveGenreFromBookCommand { get; set; }
        public DelegateCommand AddStateToBookCommand { get; set; }
        public DelegateCommand RemoveStateFromBookCommand { get; set; }

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

        private ObservableCollection<string> m_States;

        public ObservableCollection<string> States
        {
            get { return m_States; }
            set { SetProperty(ref m_States, value); }
        }

        private ObservableCollection<BookStateModel> m_BookStates;

        public ObservableCollection<BookStateModel> BookStates
        {
            get { return m_BookStates; }
            set { SetProperty(ref m_BookStates, value); }
        }
        #endregion

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private string m_Id;

        public string Id
        {
            get { return m_Id; }
            set { SetProperty(ref m_Id, value); }
        }

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

        private string m_State;

        public string State
        {
            get { return m_State; }
            set { SetProperty(ref m_State, value); }
        }

        private string m_StateQuantity;

        public string StateQuantity
        {
            get { return m_StateQuantity; }
            set { SetProperty(ref m_StateQuantity, value); }
        }

        private BookStateModel m_BookState;

        public BookStateModel BookState
        {
            get { return m_BookState; }
            set { SetProperty(ref m_BookState, value); }
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

        public AdminBookModifyViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            DeletedStates = new();

            LoadCommand = new(async () => await LoadAsync());
            AddGenreToBookCommand = new(AddGenreToBook);
            RemoveGenreFromBookCommand = new(RemoveGenreFromBook);
            AddStateToBookCommand = new(AddStateToBook);
            RemoveStateFromBookCommand = new(RemoveStateFromBook);
            SaveCommand = new(async () => await Save());
        }

        public async Task Save()
        {
            bool genresValid = (BookGenres.Count > 0);
            CheckError("Genres", "Un livre doit avoir au moins un genre !", genresValid);
            bool bookValid = Book.IsValid();
            CheckError("Livre", "Un ou des champs du livres sont invalides !", bookValid);
            bool statesValid = (BookStates.Count > 0);
            if (genresValid == true && bookValid == true && statesValid == true)
            {
                Random rnd = new Random();
                string newPath = $"../../../Images/{Title}-{Editor}-{Format}-{rnd.Next(0, 100)}.jpg";
                File.Copy(ImagePath, newPath, true);

                var bookFromRepo = await m_Repository.GetBookAsync(Book.Id);
                m_Mapper.Map(Book, bookFromRepo);
                bookFromRepo.CategoryId = Category.Id;
                bookFromRepo.Preface = newPath;

                var bookGenresFromRepo = await m_Repository.GetBookGenreEntitiesAsync(bookFromRepo.Id);

                foreach (var genre in BookGenres)
                {
                    if (!bookGenresFromRepo.Any(x => x.BookId == bookFromRepo.Id && x.GenreId == genre.Id))
                    {
                        bookGenresFromRepo.Add(new BookGenreEntity()
                        {
                            Book = bookFromRepo,
                            Genre = m_Mapper.Map<GenreEntity>(genre)
                        });
                    }
                }

                foreach (var bookState in DeletedStates)
                {
                    var copy = await m_Repository.GetBookCopyAsync(bookState.Id);
                    if (copy is not null)
                    {
                        m_Repository.DeleteBookCopy(copy);
                    }
                }

                foreach (var bookState in BookStates)
                {
                    if (bookState.Id == 0)
                    {
                        BookCopyEntity bookCopy = new BookCopyEntity()
                        {
                            State = bookState.State,
                            Quantity = (uint)bookState.Quantity,
                            Book = bookFromRepo,
                            BookId = bookFromRepo.Id
                        };

                        m_Repository.AddBookCopy(bookCopy);
                    }
                    else
                    {
                        var bookCopyFromRepo = await m_Repository.GetBookCopyAsync(bookState.Id);
                        m_Mapper.Map(bookState, bookCopyFromRepo);
                    }
                }
                await m_Repository.SaveAsync();
                GoBack();
            }
        }

        public void AddGenreToBook()
        {
            if (Genre is not null)
            {
                if (!BookGenres.Any(x => x == Genre))
                {
                    BookGenres.Add(Genre);
                    Genres.Remove(Genre);
                }
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

        public void AddStateToBook()
        {
            if (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(StateQuantity))
            {
                if (int.TryParse(StateQuantity, out int quantity))
                {
                    var state = BookStates.FirstOrDefault(x => x.State.Equals(State));
                    if (state is null)
                    {
                        BookStates.Add(new BookStateModel()
                        {
                            State = State,
                            Quantity = quantity
                        });
                    }
                    else
                    {
                        BookStateModel newstate = state;
                        newstate.Quantity = quantity;
                        BookStates.Remove(state);
                        BookStates.Add(newstate);
                        BookStates.OrderBy(x => x.State);
                    }
                    StateQuantity = string.Empty;
                }
            }
        }

        public void RemoveStateFromBook()
        {
            if (BookState is not null)
            {
                if (BookState.Id != 0)
                {
                    DeletedStates.Add(BookState);
                }
                BookStates.Remove(BookState);
            }
        }

        public override async Task LoadAsync()
        {
            var bookFromRepo = await m_Repository.GetBookAsync(BookId);
            Book = m_Mapper.Map<BookModel>(bookFromRepo);
            Book.Category = m_Mapper.Map<CategoryModel>(await m_Repository.GetCategoryAsync(bookFromRepo.CategoryId));
            var books = await m_Repository.GetBooksAsync();
            Authors = new(books.Select(p => p.Author));
            Editors = new(books.Select(p => p.Editor));
            Formats = new(books.Select(p => p.Format));
            Categories = new(m_Mapper.Map<IEnumerable<CategoryModel>>(await m_Repository.GetCategoriesAsync()));
            Genres = new(m_Mapper.Map<IEnumerable<GenreModel>>(await m_Repository.GetGenresAsync()));
            BookGenres = new(m_Mapper.Map<IEnumerable<GenreModel>>(await m_Repository.GetBookGenresAsync(BookId)));
            States = new(BookHelper.States);
            var bookCopies = await m_Repository.GetBookCopiesAsync(BookId);
            BookStates = new(m_Mapper.Map<IEnumerable<BookStateModel>>(bookCopies));
            Errors = new();
            LoadFields();
        }

        public void LoadFields()
        {
            Id = Book.Id.ToString();
            Title = Book.Title;
            Author = Authors.FirstOrDefault(x => x.Equals(Book.Author));
            Summary = Book.Summary;
            ReleaseDate = Book.ReleaseDate;
            Editor = Editors.FirstOrDefault(x => x.Equals(Book.Editor));
            Format = Formats.FirstOrDefault(x => x.Equals(Book.Format));
            Pages = Book.Pages;
            EAN = Book.EAN;
            ISBN = Book.ISBN;
            ImagePath = Path.GetFullPath(Book.PrefacePath);
            Category = Categories.FirstOrDefault(x => x.Id == Book.Category.Id);

            foreach (var bookGenre in BookGenres)
            {
                var genre = Genres.FirstOrDefault(g => g.Name.Equals(bookGenre.Name));
                if (genre is not null)
                {
                    Genres.Remove(genre);
                }
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            BookId = navigationContext.Parameters.GetValue<int>("BookId");
        }
    }
}
