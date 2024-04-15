using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Timeoff.Services
{
    internal class UsersService(IOptions<Types.Options> options)
        : IUsersService
    {
        private const int SaltBytesSize = 24;
        private const int HashBytesSize = 24; // 24 is too long for SHA1 but sufficient for > SHA224. not less than 20.
        private readonly Types.Options _options = options.Value;

        public bool Authenticate(string original, string? password)
        {
            if (original == null || password == null)
            {
                return false;
            }

            if (!Types.PhcFormat.TryParse(original, out var parts))
                return false;

            var hashed = HashPassword(password, parts);

            return parts.Equals(hashed);
        }

        public bool ShouldUpgrade(string password)
        {
            Types.PhcFormat.TryParse(password, out var parts);
            return parts.ShouldUpgrade;
        }

        private byte[] HashPassword(string password, Types.PhcFormat phc)
        {
            using var hasher = new Rfc2898DeriveBytes(password, phc.Salt, phc.IterationCount, phc.HashAlgorithm);
            return hasher.GetBytes(HashBytesSize);
        }

        public string HashPassword(string password)
        {
            var csprng = RandomNumberGenerator.Create();
            var salt = new byte[SaltBytesSize];
            csprng.GetBytes(salt);

            var parts = new Types.PhcFormat
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

        public string CreateJwt(ClaimsIdentity identity)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var signingCreds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: identity.Claims,
                audience: _options.SiteUrl,
                expires: DateTimeOffset.UtcNow.AddMinutes(5).UtcDateTime,
                signingCredentials: signingCreds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}