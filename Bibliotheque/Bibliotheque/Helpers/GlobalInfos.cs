using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Helpers
{
    public static class GlobalInfos
    {
        // Nom du fichier qui contient les informations de session
        public const string USER_SESSION_PATH = "SessionInformations";
        // Nom des paramètres partagés lors de la navigation
        public const string NAVIGATION_SERVICE = "NavigationService";
        public const string CURRENT_SESSION = "CurrentSession";
        public const string IS_CONNECTED = "IsConnected";
    }

    public enum ViewsEnum
    {
        HomeView,
        LoginView,
        RegisterView,
        BooksView,
        BookDetailsView,
        ProfileView,
        ProfileInformationsView,
        ProfileAddressView,
        ProfilePasswordView,
        ProfileBorrowView,
        ProfileHistoryView,
        AdminView,
        AdminUsersView,
        AdminBooksView,
        AdminBookAddView,
        AdminBlackListView,
        AdminBookModifyView
    }
}
