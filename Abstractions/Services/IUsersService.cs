using System.Security.Claims;

namespace Timeoff.Services
{
    public interface IUsersService
    {
        bool Authenticate(string original, string? password);
        string CreateJwt(ClaimsIdentity identity);
        string HashPassword(string password);
        bool ShouldUpgrade(string password);
        string Token();
    }
}