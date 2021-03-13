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

        private BookState m_StateEnum;

        [NotMapped]
        public BookState StateEnum
        {
            get => m_StateEnum;
            set
            {
                if (string.IsNullOrWhiteSpace(State)) State = value.StateToString();
                m_StateEnum = value;
            }
        }

        private string m_State;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(15)]
        public string State
        {
            get => m_State;
            set
            {
                if (StateEnum == BookState.Vide) StateEnum = value.StateToEnum();
                m_State = value;
            }
        }

        public int Quantity { get; set; }

        [ForeignKey("BookId")]
        public BookEntity Book { get; set; }
        public int BookId { get; set; }
    }
}
