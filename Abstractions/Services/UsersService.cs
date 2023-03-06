using System.Security.Cryptography;

namespace Timeoff.Services
{
    internal class UsersService : IUsersService
    {
        public UsersService()
        {
        }

        public bool Authenticate(string original, string? password)
        {
            if (original == null || password == null)
            {
                return false;
            }
            var parts = original.Split(':');

            if (parts.Length != 3)
            {
                return false;
            }

            var hashed = HashPassword(password, parts[1], int.Parse(parts[0]));

            return hashed == parts[2];
        }

        private string HashPassword(string password, string salt, int count)
        {
            var saltBytes = Convert.FromBase64String(salt);
            using var hasher = new Rfc2898DeriveBytes(password, saltBytes, count, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(hasher.GetBytes(24));
        }
    }
}