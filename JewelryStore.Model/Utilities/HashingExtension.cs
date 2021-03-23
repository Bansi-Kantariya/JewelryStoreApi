using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JewelryStore.Model
{
    public static class HashingExtension
    {
        public static string HashPassword(this String pwd)
        {
            using(SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pwd));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
