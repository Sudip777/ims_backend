using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace inventory_management_system.Helpers
{
    public class PasswordHasher
    {
         
        public static string HashPassword(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };
            var hash = argon2.GetBytes(32);
            var result = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);
            return Convert.ToBase64String(result);
        }

        public static bool VerifyPassword(string storedHash, string password)
        {
            var bytes = Convert.FromBase64String(storedHash);
            var salt = new byte[16];
            Buffer.BlockCopy(bytes, 0, salt, 0, 16);
            var storedPasswordHash = new byte[32];
            Buffer.BlockCopy(bytes, 16, storedPasswordHash, 0, 32);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };
            var hash = argon2.GetBytes(32);

            return hash.SequenceEqual(storedPasswordHash);
        }
    }
}

