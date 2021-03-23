using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record UserForCreationRecord(string Email, string Password, string FirstName, string LastName, string PhoneNumber, GenderRecord Gender, DateTime BirthDate);
    public record UserConnectionRecord(string Email, string Password);
    public record UserCurrentSessionRecord(Guid Id, string Email, Guid Token);
    public record UserRecord(Guid Id, string Email, string FirstName, string LastName, string PhoneNumber, string Gender, DateTime BirthDate);
}
