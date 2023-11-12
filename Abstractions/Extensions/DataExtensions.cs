using Microsoft.EntityFrameworkCore;

namespace Timeoff
{
    public static class DataFindExtensions
    {
        public static IQueryable<Entities.User> FindByEmail(this DbSet<Entities.User> users, string? email)
        {
            return users
                .Where(u => u.Email == email)
                .Where(u => u.EndDate == null || u.EndDate > DateTime.Today);
        }

        public static IQueryable<Entities.User> FindById(this DbSet<Entities.User> users, int userId)
        {
            return users
                .Where(u => u.UserId == userId);
        }

        public static IQueryable<Entities.Company> FindById(this IQueryable<Entities.Company> companies, int companyId)
        {
            return companies.Where(c => c.CompanyId == companyId);
        }

        public static IEnumerable<bool> ToEnumerable(this Entities.Schedule schedule)
        {
            return new[]
            {
                schedule.Monday == WorkingDay.WholeDay,
                schedule.Tuesday == WorkingDay.WholeDay,
                schedule.Wednesday == WorkingDay.WholeDay,
                schedule.Thursday == WorkingDay.WholeDay,
                schedule.Friday == WorkingDay.WholeDay,
                schedule.Saturday == WorkingDay.WholeDay,
                schedule.Sunday == WorkingDay.WholeDay,
            };
        }

        public static IQueryable<Entities.User> ActiveUsers(this DbSet<Entities.User> users, int companyId)
        {
            return users
                .Where(u => u.CompanyId == companyId)
                .Where(u => u.StartDate <= DateTime.Today)
                .Where(u => u.EndDate == null || u.EndDate >= DateTime.Today);
        }
    }
}