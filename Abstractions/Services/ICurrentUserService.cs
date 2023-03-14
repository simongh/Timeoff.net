using System.Security.Claims;

namespace Timeoff.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        int CompanyId { get; }

        bool IsAuthenticated { get; }
        string AuthenticationScheme { get; }

        Task SignInAsync(ClaimsPrincipal principal);
    }
}