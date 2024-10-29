using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;


namespace ConcreteIndustry.BLL.Services.Security
{
    public class PasswordHasher
    {
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize));

            return $"{hash}:{Convert.ToBase64String(salt)}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid stored hash format.");
            }

            string storedHash = parts[0];
            string storedSalt = parts[1];

            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize));

            return hash == storedHash;
        }
    }
}
