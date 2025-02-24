﻿using System;
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
        [MaxLength(100)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        [MinLength(2)]
        public string Author { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(50)]
        public string Summary { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string Editor { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(25)]
        public string Format { get; set; }

        public uint Pages { get; set; }

        [MaxLength(13)]
        [MinLength(10)]
        public string EAN { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(13)]
        [MinLength(10)]
        public string ISBN { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(500)]
        public string Preface { get; set; }

        public IList<BookGenreEntity> BookGenres { get; set; }

        // Propriétés références
        [ForeignKey("CategoryId")]
        public CategoryEntity Category { get; set; }
        public int CategoryId { get; set; }
    }
}
