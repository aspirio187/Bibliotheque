using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.DefaultData;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Bibliotheque.UI.Models.CategoryModels;
using Bibliotheque.UI.Models.GenreModels;
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
    public class AdminBookAddViewModel : BindableBase, INavigationAware
    {
        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;

        private IRegionNavigationService m_Navigation;
        private UserCurrentSessionRecord m_CurrentSession;

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand AddBookCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        public ObservableCollection<string> Authors { get; set; }
        public ObservableCollection<string> Editors { get; set; }
        public ObservableCollection<string> Formats { get; set; }
        public ObservableCollection<CategoryModel> Categories { get; set; }
        public ObservableCollection<GenreModel> Genres { get; set; }


        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

        private string m_Title;

        public string Title
        {
            get { return m_Title; }
            set { SetProperty(ref m_Title, value); }
        }

        private string m_Author;

        public string Author
        {
            get { return m_Author; }
            set { SetProperty(ref m_Author, value); }
        }

        private string m_Summary;

        public string Summary
        {
            get { return m_Summary; }
            set { SetProperty(ref m_Summary, value); }
        }

        private DateTime m_ReleaseDate;

        public DateTime ReleaseDate
        {
            get { return m_ReleaseDate; }
            set { SetProperty(ref m_ReleaseDate, value); }
        }

        private string m_Editor;

        public string Editor
        {
            get { return m_Editor; }
            set { SetProperty(ref m_Editor, value); }
        }

        private string m_Collection;

        public string Collection
        {
            get { return m_Collection; }
            set { SetProperty(ref m_Collection, value); }
        }

        private string m_Format;

        public string Format
        {
            get { return m_Format; }
            set { SetProperty(ref m_Format, value); }
        }

        private int m_Pages;

        public int Pages
        {
            get { return m_Pages; }
            set { SetProperty(ref m_Pages, value); }
        }

        private string m_EAN;

        public string EAN
        {
            get { return m_EAN; }
            set { SetProperty(ref m_EAN, value); }
        }

        private string m_ISBN;

        public string ISBN
        {
            get { return m_ISBN; }
            set { SetProperty(ref m_ISBN, value); }
        }

        private CategoryModel m_Category;

        public CategoryModel Category
        {
            get { return m_Category; }
            set { SetProperty(ref m_Category, value); }
        }

        private GenreModel m_Genre;

        public GenreModel Genre
        {
            get { return m_Genre; }
            set { SetProperty(ref m_Genre, value); }
        }

        private string m_ImagePath;

        public string ImagePath
        {
            get { return m_ImagePath; }
            set { SetProperty(ref m_ImagePath, value); }
        }


        public AdminBookAddViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            // Chargement des propriétés du repository
            Task.Run(Load);
            // Chargement des commandes
            AddBookCommand = new(AddBook);
        }

        public async Task Load()
        {
            var books = await m_Repository.GetBooksAsync();
            Authors = new(books.Select(p => p.Author));
            Editors = new(books.Select(p => p.Editor));
            Formats = new(books.Select(p => p.Format));
            Categories = new(m_Mapper.Map<IEnumerable<CategoryModel>>(await m_Repository.GetCategoriesAsync)));
            Genres = new(m_Mapper.Map<IEnumerable<GenreModel>>(await m_Repository.GetGenresAsync()));
        }

        public void AddBook()
        {

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
