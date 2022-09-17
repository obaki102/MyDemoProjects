﻿using MyDemoProjects.Shared.DTO;
using System.Security.Cryptography;
using System.Text;

namespace MyDemoProjects.Server.Application.Features.Authentication.Utility
{
    public static class Extensions
    {



    }

    public static class PasswordHash
    {
        public static void Create(string passsword, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(passsword));
            }
        }

        public static bool Verify(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash =
                    hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
