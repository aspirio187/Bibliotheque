using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(35)]
        [MinLength(5)]
        public string Name { get; set; }

        public IList<BookGenreEntity> BookGenres { get; set; }
    }
}
