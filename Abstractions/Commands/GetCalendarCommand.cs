using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record GetCalendarCommand : IRequest<ResultModels.CalendarViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;

        public bool ShowFullYear { get; init; }
    }

    internal class GetCalendarCommandHandler : IRequestHandler<GetCalendarCommand, ResultModels.CalendarViewModel>
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

        public async Task<ResultModels.CalendarViewModel> Handle(GetCalendarCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                 .FindById(_currentUserService.UserId)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

            return new()
            {
                CurrentYear = request.Year,
                ShowFullYear = request.ShowFullYear,
                Name = user.Fullname,
                Calendar = await GetCalendarAsync(user.UserId, user.CompanyId, request.Year, request.ShowFullYear),
                AllowanceSummary = await _dataContext.GetAllowanceAsync(user.UserId, request.Year)
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