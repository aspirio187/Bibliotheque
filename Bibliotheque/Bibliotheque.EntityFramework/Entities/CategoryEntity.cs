using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
