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
            var allowance = await dataContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    u.Team.Allowance,
                    u.CompanyId,
                    Adjustment = u.Adjustments.Where(a => a.Year == year).FirstOrDefault(),
                })
                .FirstAsync();

            var leaves = await dataContext.LeaveTypes
               .Where(lt => lt.CompanyId == allowance.CompanyId)
               .OrderBy(lt => lt.SortOrder)
               .Select(lt => new ResultModels.LeaveSummaryResult
               {
                   AffectsAllowance = lt.UseAllowance,
                   Name = lt.Name,
                   Allowance = lt.Limit,
                   Total = lt.Leaves.Where(l => l.UserId == userId).Sum(l => l.Days),
               })
               .Where(s => s.Total > 0)
               .ToArrayAsync();

            return new()
            {
                TotalAllowance = allowance.Allowance,
                CarryOver = allowance.Adjustment?.CarriedOverAllowance ?? 0,
                Adjustment = allowance.Adjustment?.Adjustment ?? 0,
                PreviousYear = year - 1,
                LeaveSummary = leaves,
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

        public static void UpdateSchedule(this Entities.Schedule schedule, Types.ScheduleModel model)
        {
            static WorkingDay ToWorkingDay(bool isWorkingDay) => isWorkingDay ? WorkingDay.WholeDay : WorkingDay.None;

            schedule.Monday = ToWorkingDay(model.Monday);
            schedule.Tuesday = ToWorkingDay(model.Tuesday);
            schedule.Wednesday = ToWorkingDay(model.Wednesday);
            schedule.Thursday = ToWorkingDay(model.Thursday);
            schedule.Friday = ToWorkingDay(model.Friday);
            schedule.Saturday = ToWorkingDay(model.Saturday);
            schedule.Sunday = ToWorkingDay(model.Sunday);
        }

        public static IQueryable<Entities.User> ActiveUsers(this DbSet<Entities.User> users, int companyId)
        {
            return users
                .Where(u => u.CompanyId == companyId)
                .Where(u => u.StartDate <= DateTime.Today)
                .Where(u => u.EndDate == null || u.EndDate >= DateTime.Today);
        }

        public static async Task<IEnumerable<ResultModels.CalendarMonthResult>> GetCalendarAsync(this IDataContext dataContext, int userId, int companyId, int year, bool fullYear)
        {
            var calendar = new List<ResultModels.CalendarMonthResult>();
            int months;
            DateTime startDate;

            if (fullYear)
            {
                startDate = new DateTime(year, 1, 1);
                months = 12;
            }
            else
            {
                startDate = new DateTime(year, DateTime.Today.Month, 1);
                months = 4;
            }

            var absences = await dataContext.Leaves
                .Where(a => a.UserId == userId & a.DateStart >= startDate && a.DateStart < startDate.AddMonths(months + 1))
                .ToArrayAsync();

            var holidays = await dataContext.PublicHolidays
                .Where(h => h.CompanyId == companyId && h.Date >= startDate && h.Date < startDate.AddMonths(months + 1))
                .Select(h => new ResultModels.PublicHolidayResult
                {
                    Id = h.PublicHolidayId,
                    Date = h.Date,
                    Name = h.Name,
                })
                .ToArrayAsync();

            for (int i = 0; i < months; i++)
            {
                calendar.Add(ResultModels.CalendarMonthResult.FromDate(startDate.AddMonths(i), absences, holidays));
            }

            return calendar;
        }
    }
}