using Microsoft.EntityFrameworkCore;

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

        public static async Task<Teams.TeamsViewModel> QueryTeams(this IDataContext dataContext, int companyId)
        {
            var teams = await dataContext.Teams
                 .Where(d => d.CompanyId == companyId)
                 .OrderBy(d => d.Name)
                 .Select(d => new ResultModels.TeamResult
                 {
                     Id = d.TeamId,
                     Name = d.Name,
                     Allowance = d.Allowance,
                     EmployeeCount = d.Users.Count(),
                     IsAccruedAllowance = d.IsAccrued,
                     IncludePublicHolidays = d.IncludePublicHolidays,
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
                Teams = teams,
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
                    Schedule = c.Schedule.ToEnumerable(),
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

        public static async Task<Users.DetailsViewModel?> GetUserDetailsAsync(this IDataContext dataContext, int companyId, int userId)
        {
            return await dataContext.Users
                .Where(u => u.CompanyId == companyId)
                .Where(u => u.UserId == userId)
                .Select(u => new Users.DetailsViewModel
                {
                    Id = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate,
                    AutoApprove = u.AutoApprove,
                    IsActive = u.IsActivated && (u.EndDate == null || u.EndDate > DateTime.Today),
                    IsAdmin = u.IsAdmin,
                    TeamId = u.TeamId,
                    Email = u.Email,
                    CompanyName = u.Company.Name,
                    DateFormat = u.Company.DateFormat,
                    Teams = u.Company.Departments
                    .OrderBy(d => d.Name)
                    .Select(d => new ResultModels.ListItem
                    {
                        Id = d.TeamId,
                        Value = d.Name,
                    }),
                })
                .FirstOrDefaultAsync();
        }

        public static async Task<Users.ScheduleViewModel> GetUserScheduleAsync(this IDataContext dataContext, int companyId, int userId)
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

        public static async Task<Users.CreateViewModel> GetCreateViewModelAsync(this IDataContext dataContext, int companyId)
        {
            return await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new Users.CreateViewModel
                {
                    CompanyName = c.Name,
                    DateFormat = c.DateFormat,
                    Teams = c.Departments
                        .OrderBy(t => t.Name)
                        .Select(t => new ResultModels.ListItem
                        {
                            Id = t.TeamId,
                            Value = t.Name,
                        })
                })
                .FirstAsync();
        }

        public static async Task<Users.AbsencesViewModel> GetAbsencesAync(this IDataContext dataContext, int companyId, int userId)
        {
            var user = await dataContext.Users
                .Where(u => u.CompanyId == companyId && u.UserId == userId)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.TeamId,
                    u.IsActivated,
                    u.EndDate,
                    u.Team.IsAccrued,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException();
            }

            return new()
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                IsActive = user.IsActivated && (user.EndDate == null || user.EndDate > DateTime.Today),
                IsAccrued = user.IsAccrued,
                Summary = await dataContext.GetAllowanceAsync(userId, DateTime.Today.Year),
                LeaveRequested = await dataContext.Leaves.GetRequested(userId, DateTime.Today.Year),
            };
        }

        public static async Task<Users.UsersViewModel> QueryUsers(this IDataContext dataContext, int companyId, int? team)
        {
            var query = dataContext.Users
                .Where(u => u.CompanyId == companyId);

            if (team.HasValue)
            {
                query = query.Where(u => u.TeamId == team.Value);
            }
            var year = DateTime.Today.Year;

            var users = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.UserInfoResult
                {
                    Id = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    TeamId = u.TeamId,
                    TeamName = u.Team.Name,
                    IsActive = u.IsActive,
                    IsAdmin = u.IsAdmin,
                    AllowanceCalculator = new()
                    {
                        DaysUsed = u.Leave
                            .Where(a => a.DateStart.Year == year && a.DateEnd.Year == year)
                            .Where(a => a.Status == LeaveStatus.Approved)
                            .Sum(a => a.Days),
                        Start = u.StartDate,
                        End = u.EndDate,
                        Acrrue = u.Team.IsAccrued,
                        Adjustment = u.Adjustments
                            .Where(a => a.Year == year)
                            .FirstOrDefault()!,
                        Allowance = u.Team.Allowance,
                    }
                })
                .ToArrayAsync();

            var company = await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new
                {
                    c.Name,
                    Teams = c.Departments
                        .OrderBy(d => d.Name)
                        .Select(d => new ResultModels.ListItem
                        {
                            Id = d.TeamId,
                            Value = d.Name,
                        })
                })
                .FirstAsync();

            return new()
            {
                CompanyName = company.Name,
                TeamId = team,
                Teams = company.Teams,
                Users = users,
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
    }
}