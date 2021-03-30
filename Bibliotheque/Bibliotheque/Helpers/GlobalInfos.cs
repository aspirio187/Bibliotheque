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
        public static readonly string UserSessionPath = "SessionInformations";
        // Nom des paramètres partagés lors de la navigation
        public static readonly string NavigationService = "NavigationService";
        public static readonly string CurrentSession = "CurrentSession";
        public static readonly string IsConnected = "IsConnected";
    }

    public enum ViewsEnum
    {
        HomeView,
        LoginView,
        RegisterView,
        ProfileView,
        ProfileInformationsView,
        ProfileAddressView,
        ProfilePasswordView,
        ProfileBorrowView,
        ProfileHistoryView,
        AdminView,
    }
}
