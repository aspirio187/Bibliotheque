using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class BookCopyModel : Model<int, BookCopyModel, BookCopyModel.Properties>
    {
        public enum Properties
        {
            Book,
            State,
            Quantity
        }

        private BookModel m_Book;

        public BookModel Book
        {
            get { return m_Book; }
            set { DefineBook(value); }
        }

        public ChangeResult<BookModel> DefineBook(BookModel newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<BookModel>.Failed(this, Properties.Book, "Le livre est requis !", m_Book, newValue);
            }

            if (!newValue.IsValid())
            {
                return ChangeResult<BookModel>.Failed(this, Properties.Book, "Un ou des propriétés du livres sont incorrects !", m_Book, newValue);
            }

            var initialValue = m_Book;
            m_Book = newValue;
            return ChangeResult<BookModel>.Succeded(this, Properties.Book, initialValue, newValue);
        }

        private string m_State;

        public string State
        {
            get { return m_State; }
            set { DefineState(value); }
        }

        public ChangeResult<string> DefineState(string newValue)
        {
            if (newValue is null)
            {
                return ChangeResult<string>.Failed(this, Properties.State, "L'état de la copie du livre est requis !", m_State, newValue);
            }

            newValue = newValue.Trim();
            if (!BookHelper.StateExist(newValue))
            {
                return ChangeResult<string>.Failed(this, Properties.State, "L'état de la copie du livre définit n'existe pas !", m_State, newValue);
            }

            var initialValue = m_State;
            m_State = newValue;
            return ChangeResult<string>.Succeded(this, Properties.State, initialValue, newValue);
        }

        private int m_Quantity;

        public int Quantity
        {
            get { return m_Quantity; }
            set { DefineQuantity(value); }
        }

        public ChangeResult<int> DefineQuantity(int newValue)
        {
            if (newValue < 0)
            {
                return ChangeResult<int>.Failed(this, Properties.Quantity, "La quantité de copie ne peut pas être négative !", m_Quantity, newValue);
            }

            var initialValue = m_Quantity;
            m_Quantity = newValue;
            return ChangeResult<int>.Succeded(this, Properties.Quantity, initialValue, newValue);
        }

        public override string ToString()
        {
            return $"{State} - {Quantity}";
        }
    }
}
