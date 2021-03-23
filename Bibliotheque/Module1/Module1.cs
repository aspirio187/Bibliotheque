using Module1.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Module1
{
    public class Module1 : IModule
    {
        private readonly IRegionManager _regionManager;

        public Module1(IRegionManager regionManager)
        {
            _regionManager = regionManager ??
                throw new ArgumentNullException(nameof(regionManager));
        }


        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}