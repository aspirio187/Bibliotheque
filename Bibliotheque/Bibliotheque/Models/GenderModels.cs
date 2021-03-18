using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record GenderRecord(string Name)
    {
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
