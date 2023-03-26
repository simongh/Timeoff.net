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

        public static async Task<ResultModels.AllowanceSummaryResult> GetAllowanceAsync(this IDataContext dataContext, int userId, int year)
        {
            var used = dataContext.Leaves
                .Where(u => u.UserId == userId)
                .Where(a => a.DateStart.Year == year)
                .Select(a => (a.DateStart - a.DateEnd))
                .AsEnumerable()
                .Sum(a => a.TotalDays);

            var allowance = await dataContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => u.Department.Allowance + u.Adjustments
                    .Where(a => a.Year == year).Sum(a => a.CarriedOverAllowance + a.Adjustment))
                .FirstAsync();

            return new()
            {
                TotalAllowance = allowance,
                Used = used,
            };
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
    }
}