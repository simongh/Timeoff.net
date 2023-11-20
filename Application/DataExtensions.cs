using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application
{
    internal static class DataExtensions
    {
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

        public static async Task<Teams.TeamsViewModel> QueryTeams(this IDataContext dataContext, int companyId)
        {
            var teams = await dataContext.Teams
                 .Where(d => d.CompanyId == companyId)
                 .OrderBy(d => d.Name)
                 .Select(d => new Teams.TeamResult
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
                        .Select(l => new Settings.LeaveTypeResult
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

        public static async Task<UserDetails.DetailsViewModel?> GetUserDetailsAsync(this IDataContext dataContext, int companyId, int userId)
        {
            return await dataContext.Users
                .Where(u => u.CompanyId == companyId)
                .Where(u => u.UserId == userId)
                .Select(u => new UserDetails.DetailsViewModel
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
                    Teams = u.Company.Teams
                    .OrderBy(d => d.Name)
                    .Select(d => new ResultModels.ListItem
                    {
                        Id = d.TeamId,
                        Value = d.Name,
                    }),
                })
                .FirstOrDefaultAsync();
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
                .Select(u => new Users.UserInfoResult
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
                        IsAccrued = u.Team.IsAccrued,
                        Adjustment = u.Adjustments
                            .Where(a => a.Year == year)
                            .Any()
                            ? u.Adjustments
                                .Where(a => a.Year == year)
                                .FirstOrDefault()!
                                .Adjustment
                            : 0,
                        Allowance = u.Team.Allowance,
                    }
                })
                .ToArrayAsync();

            var company = await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new
                {
                    c.Name,
                    Teams = c.Teams
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