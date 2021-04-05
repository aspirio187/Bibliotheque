using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class BorrowModel : Model<int, BorrowModel, BorrowModel.Properties>
    {
        public enum Properties
        {
            Book,
            StartDate,
            EndDate
        }
    }
}
