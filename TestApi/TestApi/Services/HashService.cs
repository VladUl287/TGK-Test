﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace TestApi.Services
{
    public static class HashService
    {
        public static string Hash(string password, string key)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: password,
                 salt: Encoding.UTF8.GetBytes(key),
                 prf: KeyDerivationPrf.HMACSHA256,
                 iterationCount: 100000,
                 numBytesRequested: 256 / 8));
        }
    }
}