using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record UserCurrentSessionRecord(Guid Id, string Email, Guid Token);
}
