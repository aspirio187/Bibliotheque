using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Authentication
{
    public class LoginResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public Guid Token { get; set; }
    }
}
