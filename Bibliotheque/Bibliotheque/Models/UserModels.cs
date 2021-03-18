using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record UserForCreationRecord(string Email, string Password, string FirstName, string LastName, GenderRecord Gender, DateTime BirthDate, AddressForCreationRecord Address);
    public record UserConnectionRecord(string Email, string Password);
    public record UserCurrectSessionRecord(Guid Id, string Email, Guid Token);
}
