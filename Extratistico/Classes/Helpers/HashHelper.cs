using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace Extratistico.Classes.Helpers
{
    public class HashHelper
    {
        private static string salt = "WebTemplate2013*";

        public static string ComputeHash(string s)
        {
            SHA512Managed hasher = new SHA512Managed();
            Byte[] salted = System.Text.Encoding.UTF8.GetBytes(string.Concat(s, salt));
            Byte[] returnBytes = hasher.ComputeHash(salted);
            hasher.Clear();
            return Convert.ToBase64String(returnBytes);
        }

        public static string GeneratePassword(int passwordLength, bool strongPassword)
        {
            Random Random = new Random();
            int seed = Random.Next(1, int.MaxValue);
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string specialCharacters = @"!#$@_";

            var chars = new char[passwordLength];
            var rd = new Random(seed);

            for (var i = 0; i < passwordLength; i++)
            {
                // Se for usar caracteres especiais
                if (strongPassword && i % Random.Next(3, passwordLength) == 0)
                {
                    chars[i] = specialCharacters[rd.Next(0, specialCharacters.Length)];
                }
                else
                {
                    chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
                }
            }

            return new string(chars);
        }
    }
}