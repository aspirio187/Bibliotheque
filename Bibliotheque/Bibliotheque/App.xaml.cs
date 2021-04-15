using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Bibliotheque.EntityFramework.DbContexts;
using Bibliotheque.EntityFramework.Services.Authentication;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

namespace Bibliotheque
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Injection des dépendances
            containerRegistry.Register<LibraryContext>();
            containerRegistry.Register<ILibraryRepository, LibraryRepository>();
            containerRegistry.Register<IMapper>(InitAutoMapper);
            containerRegistry.Register<IUserService, UserService>();

            // Déclaration des vues dans la navigation
            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<RegisterView>();
            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<BooksView>();
            containerRegistry.RegisterForNavigation<BookDetailsView>();

            containerRegistry.RegisterForNavigation<ProfileView>();
            containerRegistry.RegisterForNavigation<ProfileInformationsView>();
            containerRegistry.RegisterForNavigation<ProfileAddressView>();
            containerRegistry.RegisterForNavigation<ProfilePasswordView>();

            containerRegistry.RegisterForNavigation<AdminView>();
            containerRegistry.RegisterForNavigation<AdminBooksView>();
            containerRegistry.RegisterForNavigation<AdminBookAddView>();
            containerRegistry.RegisterForNavigation<AdminBookModifyView>();
            containerRegistry.RegisterForNavigation<AdminUsersView>();
        }

        private IMapper InitAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            IMapper mapper = new Mapper(configuration);
            return mapper;
        }
    }
}