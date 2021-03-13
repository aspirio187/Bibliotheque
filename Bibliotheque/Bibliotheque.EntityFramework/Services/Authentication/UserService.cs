using Bibliotheque.EntityFramework.DbContexts;
using Bibliotheque.EntityFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly LibraryContext m_Context;

        public UserService(LibraryContext context)
        {
            m_Context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            var user = m_Context.Users.Where(userRepo => userRepo.Email == loginRequest.Email).FirstOrDefault();

            if (user == null)
                return null;

            var passwordHash = HashingHelper.HashUsingPbkdf2(user.Password, user.Email);

            if (user.Password != passwordHash)
                return null;

            return new() { UserId = user.Id, Email = loginRequest.Email, Token = user.Token };
        }
    }
}
