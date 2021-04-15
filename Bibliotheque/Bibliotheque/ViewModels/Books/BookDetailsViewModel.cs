using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Models;
using Prism.Commands;
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
    public class BookDetailsViewModel : BaseViewModel
    {
        public int BookId { get; private set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/
        public DelegateCommand BorrowCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/
        private ObservableCollection<GenreModel> m_Genres;

        public ObservableCollection<GenreModel> Genres
        {
            get { return m_Genres; }
            set { SetProperty(ref m_Genres, value); }
        }

        private ObservableCollection<BookStateModel> m_States;

        public ObservableCollection<BookStateModel> States
        {
            get { return m_States; }
            set { SetProperty(ref m_States, value); }
        }
        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private BookModel m_Book;

        public BookModel Book
        {
            get { return m_Book; }
            set { SetProperty(ref m_Book, value); }
        }

        private CategoryModel m_Category;

        public CategoryModel Category
        {
            get { return m_Category; }
            set { SetProperty(ref m_Category, value); }
        }

        private bool m_CanBook = true;

        public bool CanBorrow
        {
            get { return m_CanBook; }
            set { SetProperty(ref m_CanBook, value); }
        }

        private BookStateModel m_State;

        public BookStateModel State
        {
            get { return m_State; }
            set { SetProperty(ref m_State, value); }
        }

        private DateTime m_EndDate = DateTime.Now;

        public DateTime EndDate
        {
            get { return m_EndDate; }
            set
            {
                CanBorrow = CalculateSupplement(value);
                SetProperty(ref m_EndDate, value);
            }
        }

        private decimal m_Supplement;

        public decimal Supplement
        {
            get { return m_Supplement; }
            set { SetProperty(ref m_Supplement, value); }
        }

        private string m_PrefacePath;

        public string PrefacePath
        {
            get { return m_PrefacePath; }
            set { SetProperty(ref m_PrefacePath, value); }
        }

        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public BookDetailsViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            LoadCommand = new(async () => await LoadAsync());
            BorrowCommand = new(async () => await Borrow());
        }

        public bool CalculateSupplement(DateTime endDate)
        {
            bool validDate = (endDate > DateTime.Now);
            CheckError("ValidDate", "La date de remise ne peut pas être avant la date d'emprunt !", validDate);
            if (validDate)
            {
                DateTime freePeriod = DateTime.Now.AddDays(30);
                if (endDate > freePeriod)
                {
                    validDate = (endDate < DateTime.Now.AddMonths(3));
                    CheckError("ValidDate", "Un livre ne peut pas être emprunté pour plus de 3 mois !", validDate);
                    if (validDate)
                    {
                        var days = (endDate - freePeriod).TotalDays;
                        var total = days / 10;
                        Supplement = Math.Round((decimal)total * 5, 2);
                        return true;
                    }
                }
                else
                {
                    Supplement = 0;
                    return true;
                }
            }
            return false;
        }

        public async Task Borrow()
        {
            if (Book.IsValid() && State.IsValid() && State.Quantity > 0)
            {
                BorrowEntity borrow = new BorrowEntity()
                {
                    UserId = CurrentSession.Id,
                    BookCopyId = State.Id,
                    BorrowingDate = DateTime.Now,
                    ExpectedDeliveryDate = EndDate,
                    DeliveryDate = null,
                    ExtraCharges = Supplement,
                };
                m_Repository.AddBorrow(borrow);
                await m_Repository.SaveAsync();
                State.Quantity--;
                GoBack();
            }
        }

        public override async Task LoadAsync()
        {
            States = new(m_Mapper.Map<IEnumerable<BookStateModel>>(await m_Repository.GetAvalaibleBookCopies(BookId)));
            Book = m_Mapper.Map<BookModel>(await m_Repository.GetBookAsync(BookId));
            Category = Book.Category;
            Genres = new(m_Mapper.Map<IEnumerable<GenreModel>>(await m_Repository.GetBookGenresAsync(BookId)));
            PrefacePath = Path.GetFullPath(Book.PrefacePath);
            if (States.Count < 1)
            {
                CanBorrow = false;
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            BookId = navigationContext.Parameters.GetValue<int>("BookId");
            if (CurrentSession is null)
            {
                CanBorrow = false;
            }
        }
    }
}
