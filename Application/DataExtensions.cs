using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application
{
    internal static class DataExtensions
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

        public static async Task<ResultModels.AllowanceSummaryResult> GetAllowanceAsync(this IDataContext dataContext, int userId, int year)
        {
            var allowance = await dataContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    u.Team.Allowance,
                    u.CompanyId,
                    Adjustment = u.Adjustments.Where(a => a.Year == year).FirstOrDefault(),
                    u.StartDate,
                    u.EndDate,
                    u.Team.IsAccrued,
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
                Allowance = allowance.Allowance,
                CarryOver = allowance.Adjustment?.CarriedOverAllowance ?? 0,
                Adjustment = allowance.Adjustment?.Adjustment ?? 0,
                Start = allowance.StartDate,
                End = allowance.EndDate,
                LeaveSummary = leaves,
                IsAccrued = allowance.IsAccrued,
                Year = year,
            };
        }

        public static async Task<IEnumerable<ResultModels.LeaveRequestedResult>> GetRequested(this DbSet<Entities.Leave> leaves, int userId, int year)
        {
            return await leaves
                .Where(l => l.UserId == userId && l.DateStart.Year == year)
                .OrderBy(l => l.DateStart)
                .AsNoTracking()
                .Select(l => new ResultModels.LeaveRequestedResult
                {
                    StartDate = l.DateStart,
                    StartPart = l.DayPartStart,
                    EndDate = l.DateEnd,
                    EndPart = l.DayPartEnd,
                    Approver = l.Approver.FirstName + " " + l.Approver.LastName,
                    Status = l.Status,
                    Type = l.LeaveType.Name,
                    Comment = l.EmployeeComment,
                    Days = l.Days,
                    Id = l.LeaveId,
                    DateFormat = l.User.Company.DateFormat,
                })
                .ToArrayAsync();
        }

        public static Types.ScheduleModel ToModel(this Entities.Schedule schedule)
        {
            return new()
            {
                Monday = schedule.Monday == WorkingDay.WholeDay,
                Tuesday = schedule.Tuesday == WorkingDay.WholeDay,
                Wednesday = schedule.Wednesday == WorkingDay.WholeDay,
                Thursday = schedule.Thursday == WorkingDay.WholeDay,
                Friday = schedule.Friday == WorkingDay.WholeDay,
                Saturday = schedule.Saturday == WorkingDay.WholeDay,
                Sunday = schedule.Sunday == WorkingDay.WholeDay,
            };
        }
    }
}