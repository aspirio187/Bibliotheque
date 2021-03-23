using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class RoleEntity
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string NormalizedName { get; set; }
    }
}
