using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Helpers
{
    public static class HashingHelper
    {
        public static string HashUsingPbkdf2(string password, string salt)
        {
            using var bytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(HashPassword(salt)), 10000, HashAlgorithmName.SHA256);
            var derivedRandomKey = bytes.GetBytes(32);
            var hash = Convert.ToBase64String(derivedRandomKey);
            return hash;
        }

        private static string HashPassword(string input)
        {
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
