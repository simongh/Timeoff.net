using System.Security.Claims;

namespace Timeoff
{
    public static class UserExtensions
    {
        public static bool ShowTeamView(this ClaimsPrincipal principal)
        {
            bool.TryParse(principal.FindFirst("showTeamView")?.Value, out bool result);

            return result;
        }
    }
}