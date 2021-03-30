using AutoMapper;
using Bibliotheque.EntityFramework.Helpers;
using Bibliotheque.EntityFramework.Services.Repositories;
using Bibliotheque.UI.DefaultData;
using Bibliotheque.UI.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bibliotheque.UI.ViewModels
{
    public class ProfilePasswordViewModel : BaseViewModel
    {
        public UserPasswordModel UserPassword { get; private set; }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand ModifyCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/


        /***************************************************/
        /******** Propriétés récupérées dans la vue ********/
        /***************************************************/
        #region Propriétés dans la vue
        private string m_OldPassword;

        public string OldPassword
        {
            get { return m_OldPassword; }
            set
            {
                var result = UserPassword.DefineOldPassword(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_OldPassword, value);
            }
        }

        private string m_NewPassword;

        public string NewPassword
        {
            get { return m_NewPassword; }
            set
            {
                var result = UserPassword.DefineNewPassword(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_NewPassword, value);
            }
        }

        private string m_NewPasswordConfirmation;

        public string NewPasswordConfirmation
        {
            get { return m_NewPasswordConfirmation; }
            set
            {
                var result = UserPassword.DefineNewPasswordConfirmation(value);
                CheckError(result.Property.ToString(), result.ErrorMessage, result.Success);
                SetProperty(ref m_NewPasswordConfirmation, value);
            }
        }
        #endregion

        public ProfilePasswordViewModel(ILibraryRepository repository, IMapper mapper)
            : base(repository, mapper)
        {
            UserPassword = new();
            ModifyCommand = new(async () => await Modify());
        }

        public async Task Modify()
        {
            var user = await m_Repository.GetUserAsync(CurrentSession.Id);
            bool result = HashingHelper.HashUsingPbkdf2(OldPassword, user.Email).Equals(user.Password);
            CheckError("Ancien mot de passe", "L'ancien mot de passe ne correspond pas !", false);
            if (result == true && UserPassword.IsValid())
            {
                user.Password = HashingHelper.HashUsingPbkdf2(NewPassword, user.Email);
                user.Token = Guid.NewGuid();
                await m_Repository.SaveAsync();
                Clear();
            }
        }

        public void Clear()
        {
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            NewPasswordConfirmation = string.Empty;
        }
    }
}
