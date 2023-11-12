using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    internal static class Extensions
    {
        public static async Task<ResultModels.CalendarResult> GetCalendarAsync(this IDataContext dataContext,
            int companyId,
            int year,
            bool fullYear)
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

            var holidays = await dataContext.PublicHolidays
                .Where(h => h.CompanyId == companyId && h.Date >= startDate && h.Date < startDate.AddMonths(months + 1))
                .Select(h => new ResultModels.PublicHolidayResult
                {
                    Id = h.PublicHolidayId,
                    Date = h.Date,
                    Name = h.Name,
                })
                .ToArrayAsync();

            return new ResultModels.CalendarResult
            {
                StartDate = startDate,
                Months = months,
                Holidays = holidays,
            };
        }
    }
}