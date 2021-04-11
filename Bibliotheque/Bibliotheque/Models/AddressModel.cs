using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class AddressModel : Model<Guid, AddressModel, AddressModel.Properties>
    {
        public enum Properties
        {
            Street,
            Appartment,
            City,
            ZipCode
        }

        private string m_Street;

        public string Street
        {
            get { return m_Street; }
            set { DefineStreet(value); }
        }

        public ChangeResult<string> DefineStreet(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Street, "La rue et le numéro sont requis !", m_Street, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length < 3)
            {
                return ChangeResult<string>.Failed(this, Properties.Street, "La rue doit faire au moins 3 caractères !", m_Street, newValue);
            }
            else if (newValue.Length > 100)
            {
                return ChangeResult<string>.Failed(this, Properties.Street, "La rue doit faire au plus 100 caractères !", m_Street, newValue);
            }

            newValue = newValue.ToLower().FirstCharToUpperForAll(new char[] { ' ', '-', '\'' });
            var initialValue = m_Street;
            m_Street = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Street, initialValue, newValue);
        }

        private string m_Appartment;

        public string Appartment
        {
            get { return m_Appartment; }
            set { DefineAppartment(value); }
        }

        public ChangeResult<string> DefineAppartment(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                newValue = newValue.Trim();
                if (newValue.Length < 1)
                {
                    return ChangeResult<string>.Failed(this, Properties.Appartment, "L'appartment doit faire au moins 1 caractère !", m_Appartment, newValue);
                }
                else if (newValue.Length > 3)
                {
                    return ChangeResult<string>.Failed(this, Properties.Appartment, "L'appartement doit faire au plus 3 caractères !", m_Appartment, newValue);
                }

                newValue = newValue.ToLower().FirstCharToUpper();
                var intiailValue = m_Appartment;
                m_Appartment = newValue;
                return ChangeResult<string>.Succeded(this, Properties.Appartment, intiailValue, newValue);
            }
            return ChangeResult<string>.Succeded(this, Properties.Appartment, m_Appartment, newValue);
        }

        private string m_City;

        public string City
        {
            get { return m_City; }
            set { DefineCity(value); }
        }

        public ChangeResult<string> DefineCity(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.City, "La ville est requise !", m_City, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length < 3)
            {
                return ChangeResult<string>.Failed(this, Properties.City, "La ville doit faire au moins 3 caractères !", m_City, newValue);
            }
            else if (newValue.Length > 50)
            {
                return ChangeResult<string>.Failed(this, Properties.City, "La ville doit faire au plus 50 caractères !", m_City, newValue);
            }

            newValue = newValue.ToLower().FirstCharToUpperForAll(new char[] { ' ', '-', '\'' });
            var initialValue = m_City;
            m_City = newValue;
            return ChangeResult<string>.Succeded(this, Properties.City, initialValue, newValue);
        }

        private string m_ZipCode;

        public string ZipCode
        {
            get { return m_ZipCode; }
            set { DefineZipCode(value); }
        }

        public ChangeResult<string> DefineZipCode(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.ZipCode, "Le code postal est requis !", m_ZipCode, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length != 4)
            {
                return ChangeResult<string>.Failed(this, Properties.ZipCode, "Le code postal doit faire exactement 4 caractères !", m_ZipCode, newValue);
            }
            else if (!newValue.IsDigit())
            {
                return ChangeResult<string>.Failed(this, Properties.ZipCode, "Le code postal ne peut être composé que de chiffres !", m_ZipCode, newValue);
            }

            var initialValue = m_ZipCode;
            m_ZipCode = newValue;
            return ChangeResult<string>.Succeded(this, Properties.ZipCode, initialValue, newValue);
        }
    }
}
