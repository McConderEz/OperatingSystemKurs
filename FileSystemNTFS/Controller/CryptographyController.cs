﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MultiuserProtection.BL.Controllers
{
    public class CryptographyController
    {
        public string GenerateHash(string passwod)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwod));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            string inputHash = GenerateHash(password);
            return inputHash.Equals(hashedPassword);
        }

    }
}
