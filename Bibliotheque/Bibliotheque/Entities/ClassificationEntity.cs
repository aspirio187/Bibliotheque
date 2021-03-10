using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.Entities
{
    public class ClassificationEntity
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MaxLength(35)]
        public string Name { get; set; }
    }
}
