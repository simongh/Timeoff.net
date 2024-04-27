using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.TeamView
{
    public record GetTeamViewCommand : IRequest<TeamViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;

        public int Month { get; init; } = DateTime.Today.Month;

        public int? Team { get; init; }

        public bool Grouped { get; init; }
    }

    internal class GetTeamViewCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetTeamViewCommand, TeamViewModel>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<TeamViewModel> Handle(GetTeamViewCommand request, CancellationToken cancellationToken)
        {
            var name = await _dataContext.Users
                .FindById(_currentUserService.UserId)
                .AsNoTracking()
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstAsync();

            var users = await GetUsersAsync(request);

            return new()
            {
                Name = name,
                CurrentDate = new DateTime(request.Year, request.Month, 1),
                Teams = await GetTeamsAsync(),
                SelectedTeam = request.Team,
                Grouped = request.Grouped,
                Users = users,
                CalendarDays = await GetAbsencesAsync(request, users),
            };
        }

        private async Task<IEnumerable<ResultModels.ListItem>> GetTeamsAsync()
        {
            var teams = _dataContext.Teams
               .Where(t => t.CompanyId == _currentUserService.CompanyId);

            if (!_currentUserService.IsAdmin)
            {
                teams = teams.Where(t => t.ManagerId == _currentUserService.UserId);
            }

            return await teams
                .OrderBy(t => t.Name)
                .Select(t => new ResultModels.ListItem
                {
                    Id = t.TeamId,
                    Value = t.Name,
                })
                .AsNoTracking()
                .ToArrayAsync();
        }

        private async Task<IEnumerable<ResultModels.CalendarDayResult>> GetAbsencesAsync(GetTeamViewCommand request, IEnumerable<UserResult> users)
        {
            var start = new DateTime(request.Year, request.Month, 1);
            var end = start.AddMonths(1);

            var holidays = await _dataContext.PublicHolidays
                .Where(h => h.CompanyId == _currentUserService.CompanyId && h.Date >= start && h.Date < end)
                .Select(h => new ResultModels.PublicHolidayResult
                {
                    Id = h.PublicHolidayId,
                    Date = h.Date,
                    Name = h.Name,
                })
                .AsNoTracking()
                .ToArrayAsync();

            var ids = users.Select(u => u.UserId).ToArray();

            var days = await _dataContext.Leaves
                .Where(l => ids.Contains(l.UserId))
                .AsNoTracking()
                .ToArrayAsync();

            var result = new List<ResultModels.CalendarDayResult>();
            for (DateTime current = start; current < end; current = current.AddDays(1))
            {
                var holiday = holidays.FirstOrDefault(h => h.Date == current);

                var leaves = days
                    .Where(a => a.DateStart >= current && a.DateEnd <= current)
                    .Select(a => new ResultModels.CalendarDayResult
                    {
                        Date = current,
                        HolidayName = holiday?.Name,
                        LeaveStatus = a.Status,
                        IsMorning = a.DayPartStart != LeavePart.Afternoon,
                        IsAfternoon = a.DayPartEnd != LeavePart.Morning,
                        LeaveId = a.LeaveId,
                        UserId = a.UserId,
                    });

                if (leaves.Any())
                {
                    result.AddRange(leaves);
                }
                else if (holiday != null)
                {
                    result.Add(new()
                    {
                        Date = current,
                        HolidayName = holiday.Name
                    });
                }
            }

            return result;
        }

        private async Task<IEnumerable<UserResult>> GetUsersAsync(GetTeamViewCommand request)
        {
            var users = _dataContext.Users.ActiveUsers(_currentUserService.CompanyId);

            if (request.Team.HasValue)
            {
                users = users.Where(u => u.TeamId == request.Team.Value);
            }

            if (!_currentUserService.IsAdmin)
            {
                users = users.Where(u => u.Team.ManagerId == _currentUserService.UserId);
            }

            return await users
                .OrderBy(u => u.FirstName + u.LastName)
                .Select(u => new UserResult
                {
                    Name = u.FirstName + " " + u.LastName,
                    UserId = u.UserId,
                    TeamId = u.TeamId,
                })
                .AsNoTracking()
                .ToArrayAsync();
        }
    }
}