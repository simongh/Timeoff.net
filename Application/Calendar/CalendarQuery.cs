using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    public record CalendarQuery : IRequest<CalendarResult>
    {
        public int User { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;
    }

    public class CalendarQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<CalendarQuery, CalendarResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<CalendarResult> Handle(CalendarQuery request, CancellationToken cancellationToken)
        {
            var id = request.User == 0 ? _currentUserService.UserId : request.User;
            var user = await _dataContext.Users
                .FindById(id)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.IsActivated,
                    u.EndDate,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            return new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActivated && (user.EndDate == null || user.EndDate > DateTime.Today),
                Days = await _dataContext.GetCalendarAsync(request.Year, true, id),
                Summary = await _dataContext.GetAllowanceAsync(id, request.Year),
                LeaveRequested = [],
            };
        }
    }
}