using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public record AddressRecord(Guid ID, string FullAddress, string FullCity);
    public record AddressForCreationRecord(string Street, string Appartment, string ZipCode, string City);
}
