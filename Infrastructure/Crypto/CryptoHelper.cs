﻿using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Crypto
{
    public class CryptoHelper
    {
        public string GetHash(string input)
        {
            using var md5Hash = MD5.Create();
            
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            
            foreach (var t in data)
                sBuilder.Append(t.ToString("x2"));

            return sBuilder.ToString();
        }
    }
}