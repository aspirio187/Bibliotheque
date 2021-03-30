using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class BookModel : Model<int, BookModel, BookModel.Properties>
    {
        public enum Properties
        {
            Title,
            Author,
            Summary,
            ReleaseDate,
            Editor,
            Format,
            Pages,
            EAN,
            ISBN,
            PrefacePath,
            Category
        }
        /***********************************************************/
        /************************ Propriétés ***********************/
        /***********************************************************/

        private string m_Title;

        public string Title
        {
            get => m_Title;
            set { DefineTitle(value); }
        }

        public ChangeResult<string> DefineTitle(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Title, "Le titre doit être défini !", m_Title, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 1)
            {
                return ChangeResult<string>.Failed(this, Properties.Title, "Le titre doit faire au moins 1 caractère !", m_Title, newValue);
            }
            else if (newValue.Length > 100)
            {
                return ChangeResult<string>.Failed(this, Properties.Title, "Le titre ne peut pas faire plus de 100 caractères !", m_Title, newValue);
            }

            var initialValue = m_Title;
            m_Title = newValue.FirstCharToUpper();
            return ChangeResult<string>.Succeded(this, Properties.Title, initialValue, newValue);
        }

        private string m_Author;

        public string Author
        {
            get => m_Author;
            set { DefineAuthor(value); }
        }

        public ChangeResult<string> DefineAuthor(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Author, "L'auteur doit être défini !", m_Author, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 2)
            {
                return ChangeResult<string>.Failed(this, Properties.Author, "L'auteur doit faire au moins 1 caractère !", m_Author, newValue);
            }
            else if (newValue.Length > 50)
            {
                return ChangeResult<string>.Failed(this, Properties.Author, "L'auteur doit faire au plus 50 caractères !", m_Author, newValue);
            }

            newValue = newValue.ToLower().FirstCharToUpperForAll(new char[] { ' ', '-' });
            var initialValue = m_Author;
            m_Author = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Author, initialValue, newValue);
        }

        private string m_Summary;

        public string Summary
        {
            get => m_Summary;
            set { DefineSummary(value); }
        }

        public ChangeResult<string> DefineSummary(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Summary, "Le résumé doit être défini !", m_Summary, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 50)
            {
                return ChangeResult<string>.Failed(this, Properties.Summary, "Le résumé doit faire au moins 50 caractères !", m_Summary, newValue);
            }

            var initialValue = m_Summary.FirstCharToUpper();
            m_Summary = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Summary, initialValue, newValue);
        }

        private DateTime m_ReleaseDate;

        public DateTime ReleaseDate
        {
            get => m_ReleaseDate;
            set { DefineReleaseDate(value); }
        }

        public ChangeResult<DateTime> DefineReleaseDate(DateTime newValue)
        {
            if (newValue == DateTime.MinValue)
            {
                return ChangeResult<DateTime>.Failed(this, Properties.ReleaseDate, "La date de sortie doit être définie !", m_ReleaseDate, newValue);
            }
            else if (newValue >= DateTime.Now)
            {
                return ChangeResult<DateTime>.Failed(this, Properties.ReleaseDate, "La date de sortie ne peut pas être que la date actuelle !", m_ReleaseDate, newValue);
            }

            var initialValue = m_ReleaseDate;
            m_ReleaseDate = newValue;
            return ChangeResult<DateTime>.Succeded(this, Properties.ReleaseDate, initialValue, newValue);
        }

        private string m_Editor;

        public string Editor
        {
            get => m_Editor;
            set { DefineEditor(value); }
        }

        public ChangeResult<string> DefineEditor(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Editor, "L'éditeur doit être défini !", m_Editor, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 1)
            {
                return ChangeResult<string>.Failed(this, Properties.Editor, "L'éditeur doit faire au moins 1 caractère !", m_Editor, newValue);
            }
            else if (newValue.Length > 25)
            {
                return ChangeResult<string>.Failed(this, Properties.Editor, "L'éditeur doit faire au plus 25 caractères !", m_Editor, newValue);
            }

            newValue = newValue.FirstCharToUpper();
            var initialValue = m_Editor;
            m_Editor = newValue;
            return ChangeResult<string>.Succeded(this, Properties.Editor, initialValue, newValue);
        }

        private string m_Format;

        public string Format
        {
            get => m_Format;
            set { DefineFormat(value); }
        }

        public ChangeResult<string> DefineFormat(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.Format, "Le format doit être défini !", m_Format, newValue);
            }

            newValue = newValue.Trim();
            if (newValue.Length < 1)
            {
                return ChangeResult<string>.Failed(this, Properties.Format, "Le format doit faire au moins 1 caractère !", m_Format, newValue);
            }
            else if (newValue.Length > 25)
            {
                return ChangeResult<string>.Failed(this, Properties.Format, "Le format doit faire au plus 25 caractères !", m_Format, newValue);
            }

            var initialValue = m_Format;
            m_Format = newValue.FirstCharToUpper();
            return ChangeResult<string>.Succeded(this, Properties.Format, initialValue, newValue);
        }

        private int m_Pages;

        public int Pages
        {
            get => m_Pages;
            set { DefinePages(value); }
        }

        public ChangeResult<int> DefinePages(int newValue)
        {
            if (newValue == 0)
            {
                return ChangeResult<int>.Failed(this, Properties.Pages, "Le nombre de pages doit être défini !", m_Pages, newValue);
            }
            else if (newValue < 0)
            {
                return ChangeResult<int>.Failed(this, Properties.Pages, "Le nombre de pages ne peut pas être négatif !", m_Pages, newValue);
            }
            else if (newValue > 50000)
            {
                return ChangeResult<int>.Failed(this, Properties.Pages, "Le nombre de page ne peut pas excéder 50 000 !", m_Pages, newValue);
            }

            var initialValue = m_Pages;
            m_Pages = newValue;
            return ChangeResult<int>.Succeded(this, Properties.Pages, initialValue, newValue);
        }

        private string m_EAN;

        public string EAN
        {
            get => m_EAN;
            set { DefineEAN(value); }
        }

        public ChangeResult<string> DefineEAN(string newValue)
        {
            if (newValue is not null)
            {
                newValue = newValue.Trim();
                if (newValue.Length != 0)
                {
                    if (newValue.Length < 10)
                    {
                        return ChangeResult<string>.Failed(this, Properties.EAN, "L'EAN doit faire au moins 10 caractères !", m_EAN, newValue);
                    }
                    else if (newValue.Length > 13)
                    {
                        return ChangeResult<string>.Failed(this, Properties.EAN, "L'EAN doit faire au plus 13 caractères !", m_EAN, newValue);
                    }
                    else if (!newValue.IsDigit())
                    {
                        return ChangeResult<string>.Failed(this, Properties.EAN, "L'EAN ne doit être composé que de chiffres !", m_EAN, newValue);
                    }

                    var initialValue = m_EAN;
                    m_EAN = newValue;
                    return ChangeResult<string>.Succeded(this, Properties.EAN, initialValue, newValue);
                }
            }
            return ChangeResult<string>.Succeded(this, Properties.EAN, m_EAN, newValue);
        }

        private string m_ISBN;

        public string ISBN
        {
            get => m_ISBN;
            set { DefineISBN(value); }
        }

        public ChangeResult<string> DefineISBN(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.ISBN, "L'ISBN doit être défini !", m_ISBN, newValue);
            }
            newValue = newValue.Trim();
            if (!newValue.IsDigit())
            {
                return ChangeResult<string>.Failed(this, Properties.ISBN, "L'ISBN ne doit être composé que de chiffres !", m_ISBN, newValue);
            }
            else if (newValue.Length < 10)
            {
                return ChangeResult<string>.Failed(this, Properties.ISBN, "L'ISBN doit faire au moins 10 caractères !", m_ISBN, newValue);
            }
            else if (newValue.Length > 13)
            {
                return ChangeResult<string>.Failed(this, Properties.ISBN, "L'ISBN doit faire au plus 13 caractères !", m_ISBN, newValue);
            }

            var initialValue = m_ISBN;
            m_ISBN = newValue;
            return ChangeResult<string>.Succeded(this, Properties.ISBN, initialValue, newValue);
        }

        private string m_PrefacePath;

        public string PrefacePath
        {
            get => m_PrefacePath;
            set { DefinePrefacePath(value); }
        }

        public ChangeResult<string> DefinePrefacePath(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.PrefacePath, "La préface doit être définie !", m_PrefacePath, newValue);
            }
            newValue = newValue.Trim();
            if (newValue.Length < 1)
            {
                return ChangeResult<string>.Failed(this, Properties.PrefacePath, "Le chamin de la préface doit faire au moins 1 caractère", m_PrefacePath, newValue);
            }
            else if (newValue.Length > 500)
            {
                return ChangeResult<string>.Failed(this, Properties.PrefacePath, "Le chemin de la préface doit faire au plus 500 caractères", m_PrefacePath, newValue);
            }
            var initialValue = m_PrefacePath;
            m_PrefacePath = newValue;
            return ChangeResult<string>.Succeded(this, Properties.PrefacePath, initialValue, newValue);
        }

        private CategoryModel m_Category;

        public CategoryModel Category
        {
            get => m_Category;
            set { DefineCategory(value); }
        }

        public ChangeResult<CategoryModel> DefineCategory(CategoryModel newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<CategoryModel>.Failed(this, Properties.Category, "La catégorie doit être définie !", m_Category, newValue);
            }

            var initialValue = m_Category;
            m_Category = newValue;
            return ChangeResult<CategoryModel>.Succeded(this, Properties.Category, initialValue, newValue);
        }
    }
}
