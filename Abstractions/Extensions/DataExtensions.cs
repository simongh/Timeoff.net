using Microsoft.EntityFrameworkCore;

namespace Timeoff
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

        public static async Task<ResultModels.BankHolidaysViewModel> GetBankHolidaysAsync(this IQueryable<Entities.Company> companies,
            int companyId,
            int year)
        {
            var company = await companies
                .FindById(companyId)
                .Select(c => new
                {
                    c.Name,
                    c.DateFormat,
                    BankHolidays = c.BankHolidays
                        .Where(h => h.Date.Year == year)
                        .Select(h => new ResultModels.BankHolidayResult
                        {
                            Id = h.BankHolidayId,
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
                    company.BankHolidays.Where(h => h.Date.Month == startDate.AddMonths(i).Month)));
            }

            return new()
            {
                CompanyName = company.Name,
                DateFormat = company.DateFormat,
                CurrentYear = year,
                Calendar = calendar,
                BankHolidays = company.BankHolidays.ToArray(),
            };
        }

        public static async Task<ResultModels.DepartmentsViewModel> QueryDepartments(this IDataContext dataContext, int companyId)
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
                .OrderBy(u => u.Name)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.ListItem
                {
                    Id = u.UserId,
                    Value = u.Name + " " + u.LastName,
                })
                .ToArrayAsync();

            return new()
            {
                Departments = departments,
                Users = users,
            };
        }

        public static async Task<ResultModels.SettingsViewModel> GetSettingsAsync(this IDataContext dataContext, int companyId)
        {
            var settings = await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new ResultModels.SettingsViewModel
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
    }
}