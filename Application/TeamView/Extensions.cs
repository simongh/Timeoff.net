using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.TeamView
{
    internal static class Extensions
    {
        public static IQueryable<Entities.User> ActiveUsers(this DbSet<Entities.User> users)
        {
            return users
                .Where(u => u.StartDate <= DateTime.Today)
                .Where(u => u.EndDate == null || u.EndDate >= DateTime.Today);
        }
    }
}