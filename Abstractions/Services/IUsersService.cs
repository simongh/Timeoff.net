namespace Timeoff.Services
{
    public interface IUsersService
    {
        bool Authenticate(string original, string? password);
    }
}