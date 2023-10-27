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
                Calendar = await GetCalendarAsync(user.UserId, user.CompanyId, request.Year, request.ShowFullYear),
                AllowanceSummary = await _dataContext.GetAllowanceAsync(user.UserId, request.Year),
                Statistics = new()
                {
                    Team = team.Team,
                    Manager = team.Manager,
                },
            };
        }

        private async Task<IEnumerable<ResultModels.CalendarMonthResult>> GetCalendarAsync(int userId, int companyId, int year, bool fullYear)
        {
            var calendar = new List<ResultModels.CalendarMonthResult>();
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

            var absences = await _dataContext.Leaves
                .Where(a => a.UserId == userId & a.DateStart >= startDate && a.DateStart < startDate.AddMonths(months + 1))
                .ToArrayAsync();

            var holidays = await _dataContext.PublicHolidays
                .Where(h => h.CompanyId == companyId && h.Date >= startDate && h.Date < startDate.AddMonths(months + 1))
                .Select(h => new ResultModels.PublicHolidayResult
                {
                    Id = h.PublicHolidayId,
                    Date = h.Date,
                    Name = h.Name,
                })
                .ToArrayAsync();

            for (int i = 0; i < months; i++)
            {
                calendar.Add(ResultModels.CalendarMonthResult.FromDate(startDate.AddMonths(i), absences, holidays));
            }

            return calendar;
        }
    }
}