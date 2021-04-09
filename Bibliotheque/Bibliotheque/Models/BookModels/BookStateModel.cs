using Bibliotheque.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class BookStateModel : Model<int, BookStateModel, BookStateModel.Properties>
    {
        public enum Properties
        {
            State,
            Quantity
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
