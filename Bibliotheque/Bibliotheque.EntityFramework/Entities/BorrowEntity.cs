using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Entities
{
    public class BorrowEntity
    {
        public int Id { get; set; }
        public DateTime BorrowingDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(2,0)")]
        public decimal ExtraCharges { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("BookCopyId")]
        public BookCopyEntity BookCopy { get; set; }
        public int BookCopyId { get; set; }
    }
}
