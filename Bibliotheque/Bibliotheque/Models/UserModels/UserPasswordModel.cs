using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class UserPasswordModel : Model<Guid, UserPasswordModel, UserPasswordModel.Properties>
    {
        public enum Properties
        {
            OldPassword,
            NewPassword,
            NewPasswordConfirmation
        }

        private string m_OldPassword;

        public string OldPassword
        {
            get { return m_OldPassword; }
            set { m_OldPassword = value; }
        }

        public ChangeResult<string> DefineOldPassword(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.OldPassword, "L'ancien mot de passe est requis !", m_OldPassword, newValue);
            }
            var initialValue = m_OldPassword;
            m_OldPassword = newValue;
            return ChangeResult<string>.Succeded(this, Properties.OldPassword, initialValue, newValue);
        }

        private string m_NewPassword;

        public string NewPassword
        {
            get { return m_NewPassword; }
            set { DefineNewPassword(value); }
        }

        public ChangeResult<string> DefineNewPassword(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.NewPassword, "Le nouveau mot de passe est requis !", m_NewPassword, newValue);
            }
            else if (!PasswordHelper.PasswordIsValid(newValue))
            {
                return ChangeResult<string>.Failed(this, Properties.NewPassword, "Le nouveau mot de passe doit contenir au moins une minuscule, une majuscule, un chiffre et un caractère sépcial !", m_NewPassword, newValue);
            }
            else if (string.IsNullOrEmpty(NewPasswordConfirmation) && !newValue.Equals(NewPasswordConfirmation))
            {
                return ChangeResult<string>.Failed(this, Properties.NewPassword, "Le nouveau mot de passe et sa confirmation sont différents !", m_NewPassword, newValue);
            }

            var initialValue = m_NewPassword;
            m_NewPassword = newValue;
            return ChangeResult<string>.Succeded(this, Properties.NewPassword, initialValue, newValue);
        }

        private string m_NewPasswordConfirmation;

        public string NewPasswordConfirmation
        {
            get { return m_NewPasswordConfirmation; }
            set { DefineNewPasswordConfirmation(value); }
        }

        public ChangeResult<string> DefineNewPasswordConfirmation(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.NewPasswordConfirmation, "La confirmation du nouveau mot de passe est requis !", m_NewPasswordConfirmation, newValue);
            }
            else if (string.IsNullOrEmpty(NewPassword) && !newValue.Equals(NewPassword))
            {
                return ChangeResult<string>.Failed(this, Properties.NewPasswordConfirmation, "La confirmation du nouveau mot de passe et sa confirmation sont différents !", m_NewPasswordConfirmation, newValue);
            }

            var initialValue = m_NewPasswordConfirmation;
            m_NewPasswordConfirmation = newValue;
            return ChangeResult<string>.Succeded(this, Properties.NewPasswordConfirmation, initialValue, newValue);
        }
    }
}
