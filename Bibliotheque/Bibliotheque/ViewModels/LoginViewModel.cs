using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand RegisterCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(async () => await Login());
            RegisterCommand = new DelegateCommand(Register);
        }

        public async Task Login()
        {
            
        }

        public void Register()
        {

        }
    }
}
