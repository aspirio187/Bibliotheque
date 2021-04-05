using AutoMapper;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.DefaultData;
using Bibliotheque.UI.Helpers;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly IRegionManager m_Region;
        private readonly string m_RegionName = "ProfileRegion";

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand NavigateToProfileInformationsCommand { get; set; }
        public DelegateCommand NavigateToAddressCommand { get; set; }
        public DelegateCommand NavigateToPasswordCommand { get; set; }
        public DelegateCommand NavigateToBorrowsCommand { get; set; }
        public DelegateCommand NavigateToHistoryCommand { get; set; }

        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/

        public ProfileViewModel(ILibraryRepository repository, IMapper mapper, IRegionManager region)
            : base(repository, mapper)
        {
            m_Region = region ??
                throw new ArgumentNullException(nameof(region));

            // Déclarations des commandes
            LoadCommand = new(Load);
            NavigateToProfileInformationsCommand = new(NavigateToProfileInformations);
            NavigateToAddressCommand = new(NavigateToAddress);
            NavigateToPasswordCommand = new(NavigateToPassword);
            NavigateToBorrowsCommand = new(NavigateToBorrows);
            NavigateToHistoryCommand = new(NavigateToHistory);
        }

        public override void Load()
        {
            Navigate(ViewsEnum.ProfileInformationsView);
        }

        public void NavigateToProfileInformations()
        {
            Navigate(ViewsEnum.ProfileInformationsView);
        }

        public void NavigateToAddress()
        {
            Navigate(ViewsEnum.ProfileAddressView);
        }

        public void NavigateToPassword()
        {
            Navigate(ViewsEnum.ProfilePasswordView);
        }

        public void NavigateToBorrows()
        {
            Navigate(ViewsEnum.ProfileBorrowView);
        }

        public void NavigateToHistory()
        {
            Navigate(ViewsEnum.ProfileHistoryView);
        }

        public override void Navigate(ViewsEnum view, Dictionary<string, object> navigationParams = null)
        {
            NavigationParameters navigationParameters = new()
            {
                { GlobalInfos.CurrentSession, CurrentSession }
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
            if (CurrentSession is null)
            {
                GoBack();
                m_NavigationService.Journal.Clear();
            }
        }
    }
}
