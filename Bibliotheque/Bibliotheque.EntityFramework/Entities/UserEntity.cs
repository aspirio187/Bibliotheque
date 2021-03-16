using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class UserEntity
    {
        // Propriétés propres
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        [EmailAddress]
        public string NormalizedEmail { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(500)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string LastName { get; set; }

        public DateTimeOffset BirthDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(5)]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        // Token crée lors de la création et mis à jours lors des modification
        // de mot de passe.
        [Required(AllowEmptyStrings = false)]
        public Guid Token { get; set; }

        // Propriétés références
        [ForeignKey("RoleId")]
        public RoleEntity Role { get; set; }
        public Guid RoleId { get; set; }
        [ForeignKey("AddressId")]
        public AddressEntity Address { get; set; }
        public Guid AddressId { get; set; }
    }
}
