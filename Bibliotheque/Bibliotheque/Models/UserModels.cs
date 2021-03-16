using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record UserForCreationRecord(string Email, string Password, string FirstName, string LastName, string Gender, DateTimeOffset BirthDate, AddressForCreationRecord Address);
}
