using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Helpers
{
    public static class GlobalInfos
    {
        public static readonly string UserSessionPath = "SessionInformations";
        public static readonly string NavigationServiceName = "NavigationService";
        public static readonly string ConnectionName = "IsConnected";

        // TODO : Stocker toutes les vues dans un enum comme pour les roles
        public static readonly string HomeView = "HomeView";
        public static readonly string LoginView = "LoginView";
        public static readonly string RegisterView = "RegisterView";
    }

    public enum ViewsEnum
    {
        HomeView,
        LoginView,
        RegisterView,
        ProfileView
    }
}
