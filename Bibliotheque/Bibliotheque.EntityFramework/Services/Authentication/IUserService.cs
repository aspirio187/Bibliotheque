using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Authentication
{
    public interface IUserService
    {
        LoginResponse Login(LoginRequest loginRequest);
    }
}
