using Store.Security.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Store.Security
{
    /// <summary>
    /// The hash provider provides functionality to hash a plain text password and validate
    /// an existing password against its hash.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Hashes a plain text password using HMACSHA512.
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <returns>A PasswordHash object containing the hashed password and salt</returns>
        public PasswordHash ComputeHash(string plainTextPassword)
        {
            using (var hmac = new HMACSHA512())
            {
                string salt = Convert.ToBase64String(hmac.Key);
                string hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword)));
                return new PasswordHash(hash, salt);
            }
        }

        /// <summary>
        /// Verifies if an existing hashed password matches a plaintext password using the provided salt.
        /// </summary>
        /// <param name="existingHashedPassword">The stored hashed password</param>
        /// <param name="plainTextPassword">The plaintext password to verify</param>
        /// <param name="salt">The salt used to create the original hash</param>
        /// <returns>True if the password matches, false otherwise</returns>
        public bool VerifyHashMatch(string existingHashedPassword, string plainTextPassword, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            using (var hmac = new HMACSHA512(saltBytes))
            {
                string hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword)));
                return hash == existingHashedPassword;
            }
        }
    }
}
