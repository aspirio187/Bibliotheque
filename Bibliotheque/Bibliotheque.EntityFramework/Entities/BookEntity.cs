using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class BookEntity
    {
        // Propriétés propres
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Author { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Summary { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Editor { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Collection { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Format { get; set; }

        public int Pages { get; set; }

        public string EAN { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string ISBN { get; set; }

        // Propriétés références
        [ForeignKey("CategoryId")]
        public CategoryEntity Category { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("ClassificationId")]
        public ClassificationEntity Classification { get; set; }
        public int ClassificationId { get; set; }
    }
}
