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
    public class ProfilePasswordViewModel : BindableBase, INavigationAware, IJournalAware
    {
        private readonly ILibraryRepository m_Repository;
        private readonly IMapper m_Mapper;

        private UserCurrentSessionRecord m_CurrentSession;

        public record ErrorRecord(Errors Error, Fields Field, string Message);

        public enum Errors
        {
            WrongPassword,
            PasswordRegex,
            PasswordMatch,
        }

        public enum Fields
        {
            OldPassword,
            NewPassword,
            NewPasswordConfirmation
        }

        /***************************************************/
        /********* Commandes s'appliquant à la vue *********/
        /***************************************************/

        public DelegateCommand ModifyCommand { get; set; }

        /***************************************************/
        /********* Collections relatives à la vue **********/
        /***************************************************/

        private ObservableCollection<ErrorRecord> m_ErrorsCollection;

        public ObservableCollection<ErrorRecord> ErrorsCollection
        {
            get { return m_ErrorsCollection; }
            set { SetProperty(ref m_ErrorsCollection, value); }
        }

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
                SetProperty(ref m_OldPassword, value);
            }
        }

        private string m_NewPassword;

        public string NewPassword
        {
            get { return m_NewPassword; }
            set
            {
                if (!PasswordIsValid(value))
                {
                    ErrorsCollection.Add(new(Errors.PasswordRegex, Fields.NewPassword, "Le mot de passe de confirmation doit contenir au moins " +
                                            "une lettre minuscule, une lettre majuscule, un chiffre et un caractère spécial!"));
                }
                else
                {
                    int i = ErrorsCollection.IndexOf(ErrorsCollection.FirstOrDefault(x => x.Error == Errors.PasswordRegex && x.Field == Fields.NewPassword));
                    if (i >= 0) ErrorsCollection.RemoveAt(i);
                }
                SetProperty(ref m_NewPassword, value);
            }
        }

        private string m_NewPasswordConfirmation;

        public string NewPasswordConfirmation
        {
            get { return m_NewPasswordConfirmation; }
            set
            {
                if (!PasswordIsValid(value))
                {
                    ErrorsCollection.Add(new(Errors.PasswordRegex, Fields.NewPasswordConfirmation, "Le mot de passe de confirmation doit contenir au moins " +
                        "une lettre minuscule, une lettre majuscule, un chiffre et un caractère spécial!"));
                }
                else
                {
                    int i = ErrorsCollection.IndexOf(ErrorsCollection.FirstOrDefault(x => x.Error == Errors.PasswordRegex && x.Field == Fields.NewPasswordConfirmation));
                    if (i >= 0) ErrorsCollection.RemoveAt(i);
                }
                if (!value.Equals(NewPassword))
                {
                    ErrorsCollection.Add(new ErrorRecord(Errors.PasswordMatch, Fields.NewPasswordConfirmation, "La confirmation du nouveau mot de passe ne" +
                        "correspond pas au nouveau mot de passe!"));
                }
                else
                {
                    int i = ErrorsCollection.IndexOf(ErrorsCollection.FirstOrDefault(x => x.Error == Errors.PasswordMatch && x.Field == Fields.NewPasswordConfirmation));
                    if (i >= 0) ErrorsCollection.RemoveAt(i);
                }
                SetProperty(ref m_NewPasswordConfirmation, value);
            }
        }
        #endregion

        public ProfilePasswordViewModel(ILibraryRepository repository, IMapper mapper)
        {
            m_Repository = repository ??
                throw new ArgumentNullException(nameof(repository));
            m_Mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            ModifyCommand = new(async () => await Modify());
        }

        public async Task Modify()
        {
            var user = await m_Repository.GetUserAsync(m_CurrentSession.Id);
            if (HashingHelper.HashUsingPbkdf2(OldPassword, user.Email).Equals(user.Password))
            {
                user.Password = HashingHelper.HashUsingPbkdf2(NewPassword, user.Email);
                user.Token = Guid.NewGuid();
                await m_Repository.SaveAsync();
                ErrorsCollection.Clear();
                Clear();
            }
            else
            {
                ErrorsCollection.Add(new ErrorRecord(Errors.WrongPassword, Fields.OldPassword, "L'ancien mot de passe ne correspond pas!"));
            }
        }

        /// <summary>
        /// Vérifie grâce aux expressions régulières si le mot de passes contient bien une lettre
        /// minuscule, une lettre majuscule, un chiffre et un caractère spécial.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>
        /// true Si le mot de passe répond bien aux conditions énumérées dans le résumé. false dans
        /// le cas contraire
        /// </returns>
        public bool PasswordIsValid(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,25}$");
        }

        public void Clear()
        {
            OldPassword = string.Empty;
            NewPassword = string.Empty;
            NewPasswordConfirmation = string.Empty;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (m_CurrentSession == null) m_CurrentSession = navigationContext.Parameters.GetValue<UserCurrentSessionRecord>(NavParameters.CurrentSessionParam);
        }

        public bool PersistInHistory()
        {
            return false;
        }
    }
}
