using Microsoft.EntityFrameworkCore;
using Timeoff.Application.Users;

namespace Timeoff.Application
{
    internal static class DataExtensions
    {
        public static async Task<PublicHoliday.PublicHolidaysViewModel> GetPublicHolidaysAsync(this IQueryable<Entities.Company> companies,
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
                        .Select(h => new ResultModels.PublicHolidayResult
                        {
                            Id = h.PublicHolidayId,
                            Date = h.Date,
                            Name = h.Name,
                        }),
                })
                .FirstAsync();

            var noLeave = Enumerable.Empty<Entities.Leave>();
            var startDate = new DateTime(year, 1, 1);
            var calendar = new List<ResultModels.CalendarMonthResult>();

            for (int i = 0; i < 12; i++)
            {
                calendar.Add(ResultModels.CalendarMonthResult.FromDate(
                    startDate.AddMonths(i),
                    noLeave,
                    company.Holidays.Where(h => h.Date.Month == startDate.AddMonths(i).Month)));
            }

            return new()
            {
                CompanyName = company.Name,
                DateFormat = company.DateFormat,
                CurrentYear = year,
                Calendar = calendar,
                PublicHolidays = company.Holidays.ToArray(),
            };
        }

        public static async Task<Departments.DepartmentsViewModel> QueryDepartments(this IDataContext dataContext, int companyId)
        {
            var departments = await dataContext.Departments
                 .Where(d => d.CompanyId == companyId)
                 .OrderBy(d => d.Name)
                 .Select(d => new ResultModels.DepartmentResult
                 {
                     Id = d.DepartmentId,
                     Name = d.Name,
                     Allowance = d.Allowance,
                     EmployeeCount = d.Users.Count(),
                     IsAccruedAllowance = d.IsAccrued,
                     IncludePublicHolidays = d.IncludeBankHolidays,
                     ManagerId = d.ManagerId!.Value,
                 })
                 .ToArrayAsync();

            var users = await dataContext.Users
                .Where(u => u.CompanyId == companyId)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.ListItem
                {
                    Id = u.UserId,
                    Value = u.FirstName + " " + u.LastName,
                })
                .ToArrayAsync();

            return new()
            {
                Departments = departments,
                Users = users,
            };
        }

        public static async Task<Settings.SettingsViewModel> GetSettingsAsync(this IDataContext dataContext, int companyId)
        {
            var settings = await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new Settings.SettingsViewModel
                {
                    Name = c.Name,
                    CarryOver = c.CarryOver,
                    Country = c.Country,
                    DateFormat = c.DateFormat,
                    TimeZone = c.TimeZone,
                    HideTeamView = c.IsTeamViewHidden,
                    ShowHoliday = c.ShareAllAbsences,
                    Schedule = new[]
                    {
                        c.Schedule.Monday == WorkingDay.WholeDay,
                        c.Schedule.Tuesday == WorkingDay.WholeDay,
                        c.Schedule.Wednesday == WorkingDay.WholeDay,
                        c.Schedule.Thursday == WorkingDay.WholeDay,
                        c.Schedule.Friday == WorkingDay.WholeDay,
                        c.Schedule.Saturday == WorkingDay.WholeDay,
                        c.Schedule.Sunday == WorkingDay.WholeDay,
                    },
                    LeaveTypes = c.LeaveTypes
                        .OrderBy(t => t.Name)
                        .Select(l => new ResultModels.LeaveTypeResult
                        {
                            Name = l.Name,
                            First = l.SortOrder == 0,
                            AutoApprove = l.AutoApprove,
                            UseAllowance = l.UseAllowance,
                            Colour = l.Colour,
                            Id = l.LeaveTypeId,
                            Limit = l.Limit,
                        })
                        .ToArray(),
                })
                .FirstAsync();

            settings.Countries = Services.CountriesService.Countries;
            settings.TimeZones = Services.TimeZoneService.TimeZones;

            return settings;
        }

        public static async Task<Settings.IntegrationApiViewModel> GetIntegrationApiAsync(this IDataContext dataContext, int companyId)
        {
            return await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new Settings.IntegrationApiViewModel
                {
                    Name = c.Name,
                    ApiKey = c.IntegrationApiToken.ToString(),
                    Enabled = c.IntegrationApiEnabled,
                })
                .FirstAsync();
        }

        public static async Task<Users.UserDetailsViewModel?> GetUserDetailsAsync(this IDataContext dataContext, int companyId, int userId)
        {
            return await dataContext.Users
                .Where(u => u.CompanyId == companyId)
                .Where(u => u.UserId == userId)
                .Select(u => new UserDetailsViewModel
                {
                    Id = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate,
                    AutoApprove = u.AutoApprove,
                    IsActive = u.IsActive,
                    IsAdmin = u.IsAdmin,
                    DepartmentId = u.DepartmentId,
                    Email = u.Email,
                    CompanyName = u.Company.Name,
                    DateFormat = u.Company.DateFormat,
                    Departments = u.Company.Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new ResultModels.ListItem
                    {
                        Id = d.DepartmentId,
                        Value = d.Name,
                    }),
                })
                .FirstOrDefaultAsync();
        }
    }
}