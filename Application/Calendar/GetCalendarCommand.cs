using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    public record GetCalendarCommand : IRequest<CalendarViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;

        public bool ShowFullYear { get; init; }
    }

    internal class GetCalendarCommandHandler : IRequestHandler<GetCalendarCommand, CalendarViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetCalendarCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                 .FindById(_currentUserService.UserId)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

            var team = await _dataContext.Teams
                .Where(t => t.TeamId == user.TeamId)
                .Select(t => new
                {
                    Team = new ResultModels.ListItem
                    {
                        Id = t.TeamId,
                        Value = t.Name,
                    },
                    Manager = new ManagerResult
                    {
                        Name = t.Manager!.FirstName + " " + t.Manager.LastName,
                        Email = t.Manager.Email,
                    },
                })
                .FirstAsync();

            return new()
            {
                CurrentYear = request.Year,
                ShowFullYear = request.ShowFullYear,
                Name = user.Fullname,
                Calendar = await _dataContext.GetCalendarAsync(user.UserId, user.CompanyId, request.Year, request.ShowFullYear),
                AllowanceSummary = await _dataContext.GetAllowanceAsync(user.UserId, request.Year),
                Statistics = new()
                {
                    Team = team.Team,
                    Manager = team.Manager,
                },
            };
        }
    }
}