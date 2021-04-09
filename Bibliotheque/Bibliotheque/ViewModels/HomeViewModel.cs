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
    public class HomeViewModel : BaseViewModel
    {
        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand<object> NavigateToBookDetailCommand { get; set; }

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
            : base(repository, mapper)
        {
            string imagePath = "../../../Images/Le throne de fer.jpg";
            string fullPath = Path.GetFullPath(imagePath);
            ImagePath = fullPath;


            NavigateToBookDetailCommand = new DelegateCommand<object>(NavigateToBookDetail);
            Load();
        }

        public override void Load()
        {
            LastAddedBooks = new(m_Mapper.Map<IEnumerable<BookMiniatureModel>>(m_Repository.GetLastBooks()));
        }

        public void NavigateToBookDetail(object bookId)
        {

        }
    }
}
