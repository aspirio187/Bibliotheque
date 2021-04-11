using Bibliotheque.EntityFramework.StaticData;
using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class UserAdminModel : Model<Guid, UserAdminModel, UserAdminModel.Properties>
    {
        public enum Properties
        {
            Selected,
            BlackListed,
            FirstName,
            LastName,
            FullAddress,
            FullCity,
            PhoneNumber,
            Email,
            BorrowQuantity,
            Role,
        }

        public bool Selected { get; set; } = false;

        public bool BlackListed { get; set; } = false;

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

            var initialValue = m_LastName;
            m_LastName = newValue;
            return ChangeResult<string>.Succeded(this, Properties.LastName, initialValue, newValue);
        }

        private string m_FullAddress;

        public string FullAddress
        {
            get { return m_FullAddress; }
            set { DefineFullAddress(value); }
        }

        public ChangeResult<string> DefineFullAddress(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.FullAddress, "L'adresse complète est requise !", m_FullAddress, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 4)
            {
                return ChangeResult<string>.Failed(this, Properties.FullAddress, "L'adresse complète doit faire au moins 4 caractères !", m_FullAddress, newValue);
            }
            else if (newValue.Length > 103)
            {
                return ChangeResult<string>.Failed(this, Properties.FullAddress, "L'addresse complète doit faire au plus 103 caractères !", m_FullAddress, newValue);
            }

            var initialValue = m_FullAddress;
            m_FullAddress = newValue;
            return ChangeResult<string>.Succeded(this, Properties.FullAddress, initialValue, newValue);
        }

        private string m_FullCity;

        public string FullCity
        {
            get { return m_FullCity; }
            set { DefineFullCity(value); }
        }

        public ChangeResult<string> DefineFullCity(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.FullCity, "La ville et le code postal sont requis !", m_FullCity, FullCity);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 7)
            {
                return ChangeResult<string>.Failed(this, Properties.FullCity, "La ville et le code postal doivent faire au moins 7 caractères ensembles !", m_FullCity, newValue);
            }
            else if (newValue.Length > 54)
            {
                return ChangeResult<string>.Failed(this, Properties.FullCity, "La ville et le code postal doivent faire au plus 54 caractères ensembles !", m_FullCity, newValue);
            }

            var initialValue = m_FullCity;
            m_FullCity = newValue;
            return ChangeResult<string>.Succeded(this, Properties.FullCity, initialValue, newValue);
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
                return ChangeResult<string>.Failed(this, Properties.PhoneNumber, "Le numéro de téléphone est requis !", m_PhoneNumber, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length != 10)
            {
                return ChangeResult<string>.Failed(this, Properties.PhoneNumber, "Le numéro de téléphone doit faire exactement 10 caractères !", m_PhoneNumber, newValue);
            }
            else if (!newValue.IsDigit())
            {
                return ChangeResult<string>.Failed(this, Properties.PhoneNumber, "Le numéro de téléphone ne peut être composé que de chiffres !", m_PhoneNumber, newValue);
            }

            var initialValue = m_PhoneNumber;
            m_PhoneNumber = newValue;
            return ChangeResult<string>.Succeded(this, Properties.PhoneNumber, initialValue, newValue);
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

            newValue = newValue.Trim();
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

        private int m_BorrowQuantity;

        public int BorrowQuantity
        {
            get { return m_BorrowQuantity; }
            set { DefineBorrows(value); }
        }

        public ChangeResult<int> DefineBorrows(int newValue)
        {
            if (newValue < 0)
            {
                return ChangeResult<int>.Failed(this, Properties.BorrowQuantity, "Le nombre d'emprunt ne peut pas être négatif !", m_BorrowQuantity, newValue);
            }

            var initialValue = m_BorrowQuantity;
            m_BorrowQuantity = newValue;
            return ChangeResult<int>.Succeded(this, Properties.BorrowQuantity, initialValue, newValue);
        }

        private string m_Role;

        public string Role
        {
            get { return m_Role; }
            set { DefineRole(value); }
        }

        public ChangeResult<string> DefineRole(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Role, "Le role est requis !", m_Role, newValue);
            }
            newValue = newValue.Trim();
            if (!RoleData.RoleExist(newValue))
            {
                return ChangeResult<string>.Failed(this, Properties.Role, "Le role donné n'existe pas !", m_Role, newValue);
            }

            var initialValue = m_Role;
            m_Role = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Role, initialValue, newValue);
        }

        private IEnumerable<BorrowModel> m_Borrows;

        public IEnumerable<BorrowModel> Borrows
        {
            get { return m_Borrows; }
            set { m_Borrows = value; }
        }
    }
}
