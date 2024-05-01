using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    internal static class Extensions
    {
        public static async Task<Types.ScheduleModel> GetUserScheduleModelAsync(this IDataContext dataContext, int userId)
        {
            var schedule = await dataContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    User = u.Schedule,
                    Company = u.Company.Schedule,
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            return (schedule.User ?? schedule.Company).ToModel();
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
    }
}