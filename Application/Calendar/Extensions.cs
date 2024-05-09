using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    internal static class Extensions
    {
        public static async Task<IEnumerable<ResultModels.DayResult>> GetCalendarAsync(this IDataContext dataContext,
            int year,
            bool fullYear,
            int userId)
        {
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

            return await dataContext.Calendar
                 .Where(h => h.Date >= startDate && h.Date < startDate.AddMonths(months + 1))
                 .Where(h => h.UserId == null || h.UserId == userId)
                 .ToModel()
                 .ToArrayAsync();
        }
    }
}