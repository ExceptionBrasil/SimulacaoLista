using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace Factorys.Tools
{
    public static class MD5tool
    {       
        /// <summary>
        /// Gera o Hash MD5 de uma string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHashMD5(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return String.Empty;
            }

            using (MD5 md5Hash = MD5.Create())
            {

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Faz a comparação de dois hash MD5, re tornando se são iguais
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyMD5(this string input, string hash)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Hash the input.
                string hashOfInput = GetHashMD5(input);

                // Create a StringComparer an compare the hashes.
                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                if (0 == comparer.Compare(hashOfInput, hash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
