using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class CategoryModel : Model<int, CategoryModel, CategoryModel.Properties>
    {
        public enum Properties
        {
            Name
        }

        private string m_Name;

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public ChangeResult<string> DefineName(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Name, "Le nom de la catégorie est requis !", m_Name, newValue);
            }
            else
            {
                newValue = newValue.Trim();
                if (newValue.Length < 5)
                {
                    return ChangeResult<string>.Failed(this, Properties.Name, "Le nom de la catégorie doit faire au moins 5 caractères !", m_Name, newValue);
                }
                else if (newValue.Length > 30)
                {
                    return ChangeResult<string>.Failed(this, Properties.Name, "Le nom de la catégorie doit faire au plus 30 caractères !", m_Name, newValue);
                }

                newValue = newValue.ToLower().FirstCharToUpperForAll(new char[] { ' ', '/', '-' });
                var initialValue = m_Name;
                m_Name = newValue;
                return ChangeResult<string>.Succeded(this, Properties.Name, initialValue, newValue);
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
