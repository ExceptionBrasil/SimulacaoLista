using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factorys
{
    public static class PasswordHash
    {
        private static PasswordHasher hash = new PasswordHasher();

        /// <summary>
        /// Hashes the specified password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static string Hash(string password)
        {
            
            return hash.HashPassword(password);

        }

        /// <summary>
        /// Verifics the specified hash password.
        /// </summary>
        /// <param name="hashPassword">The hash password.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static bool Verific (string hashPassword,string password)
        {
            PasswordVerificationResult result = hash.VerifyHashedPassword(hashPassword, password);
            return result.Equals(PasswordVerificationResult.Success);
        }
    }
}
