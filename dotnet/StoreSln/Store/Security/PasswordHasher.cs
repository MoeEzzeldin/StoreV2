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
        private const int WorkFactor = 10000;
        private const int SaltSize = 16; // 128 bits for new passwords
        private const int HashSize = 20; // Keep at 20 for backward compatibility

        /// <summary>
        /// Hashes a plain text password.
        /// </summary>
        /// <param name="plainTextPassword"></param>
        /// <returns></returns>
        public PasswordHash ComputeHash(string plainTextPassword)
        {
            // For NEW passwords, use SHA256 with 16-byte salt
            using (var rfc = new Rfc2898DeriveBytes(plainTextPassword, SaltSize, WorkFactor, HashAlgorithmName.SHA256))
            {
                // Get the Hashed Password
                byte[] hash = rfc.GetBytes(HashSize);

                // Set the SaltValue
                string salt = Convert.ToBase64String(rfc.Salt);

                // Return the Hashed Password
                return new PasswordHash(Convert.ToBase64String(hash), salt);
            }
        }

        /// <summary>
        /// Verifies if an existing hashed password matches a plaintext password (+salt).
        /// </summary>
        /// <param name="existingHashedPassword">The password we are comparing to.</param>
        /// <param name="plainTextPassword">The plaintext password being validated.</param>
        /// <param name="salt">The salt used to get the existing hashed password.</param>
        /// <returns></returns>
        public bool VerifyHashMatch(string existingHashedPassword, string plainTextPassword, string salt)
        {
            byte[] saltArray = Convert.FromBase64String(salt);

            // IMPORTANT: For backward compatibility with existing passwords in the database
            // We need to try SHA-1 first (the old default), then SHA-256 if that fails

            // First, try with SHA-1 (old default algorithm)
            try
            {
                using (var rfc = new Rfc2898DeriveBytes(plainTextPassword, saltArray, WorkFactor))
                {
                    // Get the hashed password
                    byte[] hash = rfc.GetBytes(HashSize);
                    string newHashedPassword = Convert.ToBase64String(hash);

                    if (existingHashedPassword == newHashedPassword)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // If SHA-1 fails, continue to try SHA-256
            }

            // If SHA-1 didn't match, try SHA-256 (for newer passwords)
            try
            {
                using (var rfc = new Rfc2898DeriveBytes(plainTextPassword, saltArray, WorkFactor, HashAlgorithmName.SHA256))
                {
                    // Get the hashed password
                    byte[] hash = rfc.GetBytes(HashSize);
                    string newHashedPassword = Convert.ToBase64String(hash);

                    return (existingHashedPassword == newHashedPassword);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}