using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    internal static class Extensions
    {
        public static async Task<ScheduleViewModel> GetUserScheduleAsync(this IDataContext dataContext, int companyId, int userId)
        {
            var schedule = await dataContext.Users
                .Where(u => u.CompanyId == companyId && u.UserId == userId)
                .Select(u => new
                {
                    User = u.Schedule,
                    Company = u.Company.Schedule,
                    u.FirstName,
                    u.LastName,
                    u.IsActivated,
                    u.EndDate,
                })
                 .FirstOrDefaultAsync();

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            return new()
            {
                Schedule = (schedule.User ?? schedule.Company).ToEnumerable(),
                Id = userId,
                FirstName = schedule.FirstName,
                LastName = schedule.LastName,
                IsActive = schedule.IsActivated && (schedule.EndDate == null || schedule.EndDate > DateTime.Today),
                UserSpecific = schedule.User != null,
            };
        }

        public static async Task<ScheduleModel> GetUserScheduleModelAsync(this IDataContext dataContext, int companyId, int userId)
        {
            var schedule = await dataContext.Users
                .Where(u => u.CompanyId == companyId && u.UserId == userId)
                .Select(u => new
                {
                    User = u.Schedule,
                    Company = u.Company.Schedule,
                    u.FirstName,
                    u.LastName,
                    u.IsActivated,
                    u.EndDate,
                })
                 .FirstOrDefaultAsync();

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            return (schedule.User ?? schedule.Company).ToModel();
        }

        public static void UpdateSchedule(this Entities.Schedule schedule, ScheduleModel model)
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

        public static ScheduleModel ToModel(this Entities.Schedule schedule)
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