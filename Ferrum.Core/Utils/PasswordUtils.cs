using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ferrum.Core.Utils
{
    public static class PasswordUtils
    {
        public static string GeneratePassword()
        {
            var guid = Guid.NewGuid().ToString();
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(guid));           
            return base64;            
        }

        public static string GenerateSalt()
        {
            var salt = Guid.NewGuid().ToString().Split('-')[0];
            return salt;
        }

        public static string HashPassword(string password, string salt)
        {
            var saltedPw = password + salt;
            var bytes = Encoding.UTF8.GetBytes(saltedPw);
            var hashed = SHA256.Create().ComputeHash(bytes);
            var result = Convert.ToBase64String(hashed);
            return result;
        }

        public static bool Match(string passwordPlain, string salt, string hashedPassword)
        {
            var anyNulls = new []{ passwordPlain, salt, hashedPassword }.Any(v => v == null);

            if(anyNulls)
                throw new ArgumentException("Arguments cannot be null.");

            var hashedPlain = HashPassword(passwordPlain, salt);

            var match = string.Compare(hashedPlain, hashedPassword) == 0;

            return match;
        }
    }
}
