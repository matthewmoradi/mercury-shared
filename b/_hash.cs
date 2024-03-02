using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using mercury.model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace mercury.business
{
    public class _hash
    {
        public static string sha256(string value, string _salt = entity.salt)
        {
            return sha256(new string[] { value }, _salt);
        }
        public static string sha256(string[] parameters, string _salt = entity.salt)
        {
            byte[] salt = new byte[128 / 8];
            if (_salt == null)
            {
                // generate a 128-bit salt using a secure PRNG
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }
            else
            {
                salt = Convert.FromBase64String(_salt);
            }
            string _parameters = "";
            foreach (string param in parameters)
            {
                if (param != null)
                    _parameters += param;
            }
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: _parameters,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}