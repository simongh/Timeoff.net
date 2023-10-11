using System.Security.Claims;

namespace Timeoff
{
    public static class UserExtensions
    {
        public static bool ShowTeamView(this ClaimsPrincipal principal)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == "showTeamView");

            return claim?.Value == true.ToString();
        }
    }
}