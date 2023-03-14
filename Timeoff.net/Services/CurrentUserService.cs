using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Timeoff.Services
{
    internal class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                int.TryParse(_httpContextAccessor.HttpContext!.User.FindFirstValue("userid"), out var id);
                return id;
            }
        }

        public int CompanyId
        {
            get
            {
                int.TryParse(_httpContextAccessor.HttpContext!.User.FindFirstValue("companyid"), out var id);
                return id;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string AuthenticationScheme => CookieAuthenticationDefaults.AuthenticationScheme;

        public Task SignInAsync(ClaimsPrincipal principal)
        {
            return _httpContextAccessor.HttpContext?.SignInAsync(principal) ?? Task.CompletedTask;
        }
    }
}