using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class BookGenreEntity
    {
        public int Id { get; set; }

        [ForeignKey("BookId")]
        public BookEntity Book { get; set; }
        public int BookId { get; set; }

        [ForeignKey("GenreId")]
        public GenreEntity Genre { get; set; }
        public int GenreId { get; set; }
    }
}
