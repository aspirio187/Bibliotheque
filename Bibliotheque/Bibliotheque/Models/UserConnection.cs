using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class UserConnection : Model<Guid, UserConnection, UserConnection.Properties>
    {
        public enum Properties
        {
            Email,
            Password
        }

        private string m_Email;

        public string Email
        {
            get { return m_Email; }
            set { DefineEmail(value); }
        }

        public ChangeResult<string> DefineEmail(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "Le nom d'utilisateur est requis !", m_Email, newValue);
            }
            else if (!EmailHelper.IsValidEmail(newValue))
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "L'addresse email n'est pas conforme !", m_Email, newValue);
            }
            else if (newValue.Length > 100)
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "L'addresse email doit faire au plus 100 caractères !", m_Email, newValue);
            }

            var initialValue = m_Email;
            m_Email = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Email, initialValue, newValue);
        }

        private string m_Password;

        public string Password
        {
            get { return m_Password; }
            set { DefinePassword(value); }
        }

        public ChangeResult<string> DefinePassword(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Password, "Le mot de passe est requis !", m_Password, newValue);
            }

            var initialValue = m_Password;
            m_Password = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Password, initialValue, newValue);
        }
    }
}
