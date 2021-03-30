using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record ErrorModel(string Property, string Message, bool IsValid)
    {
        // S'il faut rajouter du code
    }
}
