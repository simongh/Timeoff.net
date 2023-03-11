using System.Security.Cryptography;
using Timeoff.Types;

namespace Timeoff.Services
{
    internal class UsersService : IUsersService
    {
        private const int SaltBytesSize = 24;
        private const int HashBytesSize = 24; // 24 is too long for SHA1 but sufficient for > SHA224. not less than 20.

        public UsersService()
        { }

        public bool Authenticate(string original, string? password)
        {
            if (original == null || password == null)
            {
                return false;
            }

            if (!PhcFormat.TryParse(original, out var parts))
                return false;

            var hashed = HashPassword(password, parts);

            return parts.Equals(hashed);
        }

        public bool ShouldUpgrade(string password)
        {
            PhcFormat.TryParse(password, out var parts);
            return parts.ShouldUpgrade;
        }

        private byte[] HashPassword(string password, PhcFormat phc)
        {
            using var hasher = new Rfc2898DeriveBytes(password, phc.Salt, phc.IterationCount, phc.HashAlgorithm);
            return hasher.GetBytes(HashBytesSize);
        }

        public string HashPassword(string password)
        {
            var csprng = RandomNumberGenerator.Create();
            var salt = new byte[SaltBytesSize];
            csprng.GetBytes(salt);

            var parts = new PhcFormat
            {
                Salt = salt,
            };

            parts.Hash = HashPassword(password, parts);

            return parts.ToString();
        }

        public string Token()
        {
            var csprng = RandomNumberGenerator.Create();
            var token = new byte[40];
            csprng.GetBytes(token);

            return Convert.ToBase64String(token);
        }
    }
}