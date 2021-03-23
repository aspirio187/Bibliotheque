using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class AddressEntity
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        [MinLength(3)]
        public string Street { get; set; }

        [MaxLength(3)]
        [MinLength(1)]
        public string Appartment { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [MinLength(3)]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(4)]
        public string ZipCode { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
    }
}
