namespace Timeoff.Services
{
    public interface IUsersService
    {
        bool Authenticate(string original, string? password);
        string HashPassword(string password);
        bool ShouldUpgrade(string password);
        string Token();
    }
}