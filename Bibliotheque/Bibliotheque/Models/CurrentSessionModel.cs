using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class CurrentSessionModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
    }
}
