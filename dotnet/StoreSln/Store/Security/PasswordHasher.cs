using webstore.Security.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace webstore.Security
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
            // Create a new instance of HMACSHA512 which automatically generates a new key (salt)
            using (var hmac = new HMACSHA512())
            {
                // Get the salt
                string salt = Convert.ToBase64String(hmac.Key);
                
                // Convert the password to bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
                
                // Compute the hash
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);
                
                // Convert to Base64 string for storage
                string passwordHash = Convert.ToBase64String(hashBytes);
                
                // Return the hashed password and salt
                return new PasswordHash(passwordHash, salt);
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
            // Convert salt from Base64 string to byte array
            byte[] saltBytes = Convert.FromBase64String(salt);
            
            // Create HMACSHA512 with the provided salt as key
            using (var hmac = new HMACSHA512(saltBytes))
            {
                // Convert the plaintext password to bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
                
                // Compute the hash using the same salt
                byte[] computedHashBytes = hmac.ComputeHash(passwordBytes);
                
                // Convert to Base64 string for comparison
                string computedHashString = Convert.ToBase64String(computedHashBytes);
                
                // Compare the computed hash with the stored hash
                return existingHashedPassword == computedHashString;
            }
        }
    }
}
