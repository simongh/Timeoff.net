using System.Security.Claims;

namespace Timeoff.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        int CompanyId { get; }

        string DateFormat { get; }

        bool IsAuthenticated { get; }
        string AuthenticationScheme { get; }

        bool IsAdmin { get; }

        Task SignInAsync(ClaimsPrincipal principal);
    }
}