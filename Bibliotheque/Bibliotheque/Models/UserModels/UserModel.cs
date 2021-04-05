using Bibliotheque.UI.DefaultData;
using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class UserModel : Model<Guid, UserModel, UserModel.Properties>
    {
        public enum Properties
        {
            Email,
            FirstName,
            LastName,
            Gender,
            PhoneNumber,
            BirthDate
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
                return ChangeResult<string>.Failed(this, Properties.Email, "L'adresse email doit être définie !", m_Email, newValue);
            }
            else if (!newValue.Equals(newValue.Trim()))
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "L'addresse email ne peut pas commencer ou finir par un espace !", m_Email, newValue);
            }
            else if (!EmailHelper.IsValidEmail(newValue))
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "Le format de l'addresse email n'est pas valide !", m_Email, newValue);
            }
            else if (newValue.Length > 100)
            {
                return ChangeResult<string>.Failed(this, Properties.Email, "L'adresse email doit faire au plus 100 caractères !", m_Email, newValue);
            }

            newValue = newValue.ToLower();
            var initialValue = m_Email;
            m_Email = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Email, initialValue, newValue);
        }

        private string m_FirstName;

        public string FirstName
        {
            get { return m_FirstName; }
            set { DefineFirstName(value); }
        }

        public ChangeResult<string> DefineFirstName(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.FirstName, "Le prénom est requis !", m_FirstName, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length < 2)
            {
                return ChangeResult<string>.Failed(this, Properties.FirstName, "Le prénom doit faire au moins 2 caractères !", m_FirstName, newValue);
            }
            else if (newValue.Length > 50)
            {
                return ChangeResult<string>.Failed(this, Properties.FirstName, "Le prénom doit faire au plus 50 caractères !", m_FirstName, newValue);
            }

            newValue = newValue.ToLower().FirstCharToUpperForAll(new char[] { ' ', '-' });
            var initialValue = m_FirstName;
            m_FirstName = newValue;
            return ChangeResult<string>.Succeded(this, Properties.FirstName, initialValue, newValue);
        }

        private string m_LastName;

        public string LastName
        {
            get { return m_LastName; }
            set { DefineLastName(value); }
        }

        public ChangeResult<string> DefineLastName(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.LastName, "Le nom de famille est requis !", m_LastName, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length < 2)
            {
                return ChangeResult<string>.Failed(this, Properties.LastName, "Le nom de famille doit faire au moins 2 caractères !", m_LastName, newValue);
            }
            else if (newValue.Length > 50)
            {
                return ChangeResult<string>.Failed(this, Properties.LastName, "Le nom de famille doit faire au plus 50 caractères !", m_LastName, newValue);
            }

            newValue = newValue.ToLower().FirstCharToUpperForAll(new char[] { ' ', '-', '\'' });
            var initialValue = m_LastName;
            m_LastName = newValue;
            return ChangeResult<string>.Succeded(this, Properties.LastName, initialValue, newValue);
        }

        private DateTime m_BirthDate;

        public DateTime BirthDate
        {
            get { return m_BirthDate; }
            set { DefineBirthDate(value); }
        }

        public ChangeResult<DateTime> DefineBirthDate(DateTime newValue)
        {
            if (newValue == DateTime.MinValue)
            {
                return ChangeResult<DateTime>.Failed(this, Properties.BirthDate, "La date de naissance doit être définie !", m_BirthDate, newValue);
            }
            else if ((DateTime.Now.Year - newValue.Year) < 13)
            {
                return ChangeResult<DateTime>.Failed(this, Properties.BirthDate, "Il faut être agé de 13 ou plus pour s'inscrire !", m_BirthDate, newValue);
            }

            var initialValue = m_BirthDate;
            m_BirthDate = newValue;
            return ChangeResult<DateTime>.Succeded(this, Properties.BirthDate, initialValue, newValue);
        }

        private string m_Gender;

        public string Gender
        {
            get { return m_Gender; }
            set { DefineGender(value); }
        }

        public ChangeResult<string> DefineGender(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Gender, "Le genre est requis !", m_Gender, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length != 5)
            {
                return ChangeResult<string>.Failed(this, Properties.Gender, "Le genre doit faire exactement 5 caractères !", m_Gender, newValue);
            }
            else if (!GendersData.GetGenders().Any(x => x.Equals(newValue)))
            {
                return ChangeResult<string>.Failed(this, Properties.Gender, "Le genre rentré n'existe pas !", m_Gender, newValue);
            }

            newValue = newValue.ToLower().FirstCharToUpper();
            var initialValue = m_Gender;
            m_Gender = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Gender, initialValue, newValue);
        }

        private string m_PhoneNumber;

        public string PhoneNumber
        {
            get { return m_PhoneNumber; }
            set { DefinePhoneNumber(value); }
        }

        public ChangeResult<string> DefinePhoneNumber(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.PhoneNumber, "Le numéro de télépphone est requis !", m_PhoneNumber, newValue);
            }
            newValue = newValue.Trim();
            if (!newValue.IsDigit())
            {
                return ChangeResult<string>.Failed(this, Properties.PhoneNumber, "Le numéro de téléphone ne doit être composé que de chiffres !", m_PhoneNumber, newValue);
            }
            else if (newValue.Length != 10)
            {
                return ChangeResult<string>.Failed(this, Properties.PhoneNumber, "Le numéro de téléphone n'est pas valide !", m_PhoneNumber, newValue);
            }

            var initialValue = m_PhoneNumber;
            m_PhoneNumber = newValue;
            return ChangeResult<string>.Succeded(this, Properties.PhoneNumber, initialValue, newValue);
        }
    }
}
