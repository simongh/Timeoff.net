using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.TeamView
{
    public record TeamViewQuery : IRequest<TeamViewResult>
    {
        public int Year { get; init; } = DateTime.Today.Year;

        public int Month { get; init; } = DateTime.Today.Month;

        public int? Team { get; init; }
    }

    internal class TeamViewQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<TeamViewQuery, TeamViewResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<TeamViewResult> Handle(TeamViewQuery request, CancellationToken cancellationToken)
        {
            var canView = await _dataContext.Users
                .FindById(_currentUserService.UserId)
                .Where(u => !u.Company.IsTeamViewHidden)
                .AnyAsync();

            if (!canView)
            {
                return new()
                {
                    Errors = ["Team view is disabled"]
                };
            }

            var query = _dataContext.Users.ActiveUsers();

            if (!_currentUserService.IsAdmin)
            {
                query = query.Where(u => u.Team.ManagerId == _currentUserService.UserId);
            }
            else if (request.Team.HasValue)
            {
                query = query.Where(u => u.TeamId == request.Team.Value);
            }

            return new()
            {
                Result = await query.OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new TeamResult
                {
                    User = new()
                    {
                        Id = u.UserId,
                        Name = u.FirstName + " " + u.LastName,
                    },
                    Id = u.UserId,
                    Name = u.Team.Name,
                    Days = u.Calendar.Select(c => new ResultModels.DayResult
                    {
                        Id = c.CalendarId,
                        Name = c.Name,
                        IsHoliday = c.IsHoliday,
                        Date = c.Date,
                        DayPart = c.LeavePart,
                        Colour = c.LeaveType!.Colour,
                        Status = c.Leave!.Status,
                    }),
                    Used = u.Calendar
                        .Where(c => c.LeaveType.UseAllowance)
                        .Sum(c => c.LeavePart == LeavePart.All ? 1 : 0.5)
                })
                .ToArrayAsync()
            };
        }
    }
}