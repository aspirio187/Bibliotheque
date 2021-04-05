using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class UserSessionModel : Model<Guid, UserSessionModel, UserSessionModel.Properties>
    {
        public enum Properties
        {
            Email,
            Token
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
                return ChangeResult<string>.Failed(this, Properties.Email, "L'adresse email est requise !", m_Email, newValue);
            }

            if (!EmailHelper.IsValidEmail(newValue))
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "Le format de l'adresse email est incorrect !", m_Email, newValue);
            }
            else if (newValue.Length > 100)
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "L'adresse email doit faire au plus 100 caractères !", m_Email, newValue);
            }

            var initialValue = m_Email;
            m_Email = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Email, initialValue, newValue);
        }

        private Guid m_Token;

        public Guid Token
        {
            get { return m_Token; }
            set { DefineToken(value); }
        }

        public ChangeResult<Guid> DefineToken(Guid newValue)
        {
            if (newValue == Guid.Empty)
            {
                return ChangeResult<Guid>.Failed(this, Properties.Token, "Le token est requis !", m_Token, newValue);
            }

            var initialValue = m_Token;
            m_Token = newValue;
            return ChangeResult<Guid>.Succeded(this, Properties.Token, initialValue, newValue);
        }
    }
}
