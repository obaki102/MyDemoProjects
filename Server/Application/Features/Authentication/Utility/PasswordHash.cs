using System.Security.Cryptography;

namespace MyDemoProjects.Server.Application.Features.Authentication.Utility
{
    public static class PasswordHash
    {
        public static void Create(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
