using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Utils
{
    public static class PasswordHasher
    {
        private static string GenerateRandomSalt()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GeneratePasswordSalt(string firstName, string lastname)
        {
            var baseSalt = GenerateRandomSalt();
            var salt = new StringBuilder();

            salt.Append(firstName);
            salt.Append(baseSalt);
            salt.Append(lastname);
            return salt.ToString();
        }

        public static string HashPassword(string password, string salt)
        {
            using (var hasher = SHA512.Create())
            {
                var passwordHash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(passwordHash);
            }
        }

    }
}
