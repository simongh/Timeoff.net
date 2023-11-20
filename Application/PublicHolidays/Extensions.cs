using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    internal static class Extensions
    {
        public static async Task<PublicHolidaysViewModel> GetPublicHolidaysAsync(this IQueryable<Entities.Company> companies,
            int companyId,
            int year)
        {
            var company = await companies
                .FindById(companyId)
                .Select(c => new
                {
                    c.Name,
                    c.DateFormat,
                    Holidays = c.PublicHolidays
                        .Where(h => h.Date.Year == year)
                        .OrderBy(h => h.Date)
                        .Select(h => new ResultModels.PublicHolidayResult
                        {
                            Id = h.PublicHolidayId,
                            Date = h.Date,
                            Name = h.Name,
                        }),
                })
                .FirstAsync();

            return new()
            {
                CompanyName = company.Name,
                DateFormat = company.DateFormat,
                CurrentYear = year,
                Calendar = new()
                {
                    StartDate = new DateTime(year, 1, 1),
                    Months = 12,
                    Holidays = company.Holidays.ToArray(),
                },
            };
        }
    }
}