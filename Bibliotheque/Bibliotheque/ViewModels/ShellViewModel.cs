using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public RegionManager m_RegionManager { get; set; }

        public ShellViewModel(IRegionManager regionManager)
        {
            m_RegionManager = (RegionManager)regionManager ??
                throw new ArgumentNullException(nameof(regionManager));
        }

        public void CreateNavigationBar()
        {
            var view = NavigationView();

            m_RegionManager.Regions["NavigationBar"].Add(view);
            if (!m_RegionManager.Regions["NavigationBar"].ActiveViews.Any(v => v.Equals(view)))
            {
                m_RegionManager.Regions["NavigationBar"].Activate(view);
            }
        }
    }
}
