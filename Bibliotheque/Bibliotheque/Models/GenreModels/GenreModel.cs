using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class GenreModel : Model<int, GenreModel, GenreModel.Properties>
    {
        public enum Properties
        {
            Name
        }

        private string m_Name;

        public string Name
        {
            get => m_Name;
            set => DefineName(value);
        }

        public ChangeResult<string> DefineName(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Name, "Le nom du genre doit être défini !", m_Name, newValue);
            }
            else
            {
                newValue = newValue.Trim();
                if (newValue.Length < 5)
                {
                    return ChangeResult<string>.Failed(this, Properties.Name, "Le nom du genre doit faire au moins 5 caractères !", m_Name, newValue);
                }
                else if (newValue.Length > 35)
                {
                    return ChangeResult<string>.Failed(this, Properties.Name, "Le nom du genre doit faire au plus 35 caractères !", m_Name, newValue);
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
