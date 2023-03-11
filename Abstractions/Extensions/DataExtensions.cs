using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Timeoff
{
    internal static class DataExtensions
    {
        public static IQueryable<Entities.User> FindByEmail(this DbSet<Entities.User> users, string? email)
        {
            return users
                .Where(u => u.Email == email)
                .Where(u => u.EndDate == null || u.EndDate > DateTime.Today);
        }

        public static IQueryable<Entities.User> FindFromPrincipal(this DbSet<Entities.User> users, ClaimsPrincipal principal)
        {
            int.TryParse(principal.FindFirst("userid")?.Value, out var id);

            return users
                .Where(u => u.UserId == id);
        }
    }
}