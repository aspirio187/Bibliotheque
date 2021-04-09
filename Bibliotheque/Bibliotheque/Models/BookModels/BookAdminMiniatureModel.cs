using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Models
{
    public class BookAdminMiniatureModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool Selected { get; set; } = false;
    }
}
