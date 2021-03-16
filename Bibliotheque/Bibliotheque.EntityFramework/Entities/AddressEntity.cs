using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class AddressEntity
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Street { get; set; }

        public string Appartment { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(4)]
        public string ZipCode { get; set; }
    }
}
