using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Timeoff.Commands
{
    public record GetCalendarCommand : IRequest<ResultModels.CalendarViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;

        public bool ShowFullYear { get; init; }

        public ClaimsPrincipal User { get; set; } = null!;
    }

    internal class GetCalendarCommandHandler : IRequestHandler<GetCalendarCommand, ResultModels.CalendarViewModel>
    {
        private readonly IDataContext _dataContext;

        public GetCalendarCommandHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ResultModels.CalendarViewModel> Handle(GetCalendarCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                 .FindFromPrincipal(request.User)
                 .AsNoTracking()
                 .FirstOrDefaultAsync();

            return new()
            {
                CurrentYear = request.Year,
                ShowFullYear = request.ShowFullYear,
                Name = user.Fullname,
                Calendar = await GetCalendarAsync(user.UserId, request.Year, request.ShowFullYear),
            };
        }

        private async Task<IEnumerable<ResultModels.CalendarMonthResult>> GetCalendarAsync(int userId, int year, bool fullYear)
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

            for (int i = 0; i < months; i++)
            {
                calendar.Add(ResultModels.CalendarMonthResult.FromDate(startDate.AddMonths(i)));
            }

            return calendar;
        }
    }
}