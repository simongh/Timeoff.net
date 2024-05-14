using Microsoft.EntityFrameworkCore;

namespace Timeoff.Services
{
    internal class DaysCalculator(IDataContext dataContext) : IDaysCalculator
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<Entities.Calendar>> CalculateDaysAsync(Entities.Leave leave)
        {
            IEnumerable<DateTime> holidays;
            if (leave.User.Team.IncludePublicHolidays)
                holidays = await HolidaysAsync(leave.DateStart.Year);
            else
                holidays = [];

            return CalculateDays(leave, leave.User.Schedule ?? leave.User.Company.Schedule, holidays);
        }

        public IEnumerable<Entities.Calendar> CalculateDays(Entities.Leave leave, Entities.Schedule schedule, IEnumerable<DateTime> holidays)
        {
            var days = new List<Entities.Calendar>();
            for (DateTime i = leave.DateStart; i <= leave.DateEnd; i = i.AddDays(1))
            {
                if (holidays.Contains(i))
                    continue;

                if (!IsWorkingDay(schedule, i.DayOfWeek))
                    continue;

                var part = LeavePart.All;
                if (i == leave.DateStart && leave.DayPartStart == LeavePart.Afternoon)
                    part = LeavePart.Afternoon;
                else if (i == leave.DateEnd && leave.DayPartEnd == LeavePart.Morning)
                    part = LeavePart.Morning;

                days.Add(new Entities.Calendar
                {
                    Date = i,
                    User = leave.User,
                    CompanyId = leave.User.CompanyId,
                    LeaveTypeId = leave.LeaveTypeId,
                    LeavePart = part
                });
            }

            return days;
        }

        private bool IsWorkingDay(Entities.Schedule schedule, DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Sunday => schedule.Sunday == WorkingDay.WholeDay,
                DayOfWeek.Monday => schedule.Monday == WorkingDay.WholeDay,
                DayOfWeek.Tuesday => schedule.Tuesday == WorkingDay.WholeDay,
                DayOfWeek.Wednesday => schedule.Wednesday == WorkingDay.WholeDay,
                DayOfWeek.Thursday => schedule.Thursday == WorkingDay.WholeDay,
                DayOfWeek.Friday => schedule.Friday == WorkingDay.WholeDay,
                DayOfWeek.Saturday => schedule.Saturday == WorkingDay.WholeDay,
                _ => throw new NotSupportedException("Only valid days of the week are supported")
            };
        }

        private async Task<IEnumerable<DateTime>> HolidaysAsync(int year)
        {
            return await _dataContext.Calendar
                .Where(p => p.Date.Year == year)
                .Select(p => p.Date)
                .ToArrayAsync();
        }

        public async Task AdjustForHolidaysAsync(IEnumerable<(DateTime original, DateTime modified)> changed)
        {
            foreach (var change in changed)
            {
                if (change.original != change.modified)
                {
                    await AdjustForDate(change.original);
                }

                await AdjustForDate(change.modified);
            }

            await _dataContext.SaveChangesAsync();
        }

        private async Task AdjustForDate(DateTime when)
        {
            var leaves = await _dataContext.Leaves
                .Where(l => l.DateStart <= when && l.DateEnd >= when)
                .Where(l => l.LeaveType.UseAllowance)
                .Include(l => l.User.Schedule)
                .Include(l => l.User.Company.Schedule)
                .Include(l => l.User.Team)
                .ToArrayAsync();

            var holidays = await HolidaysAsync(when.Year);

            foreach (var leave in leaves)
            {
                await CalculateDaysAsync(leave);
            }
        }
    }
}