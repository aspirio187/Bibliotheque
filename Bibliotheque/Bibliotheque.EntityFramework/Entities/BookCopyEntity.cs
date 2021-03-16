using Bibliotheque.EntityFramework.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class BookCopyEntity
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(15)]
        public string State { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("BookId")]
        public BookEntity Book { get; set; }
        public int BookId { get; set; }
    }
}
