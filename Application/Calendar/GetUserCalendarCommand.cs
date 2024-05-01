using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    public record GetUserCalendarCommand : IRequest<CalendarResult>
    {
        public int Id { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;

        public bool FullYear { get; init; } = true;
    }

    public class GetUserCalendarComamndHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetUserCalendarCommand, CalendarResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<CalendarResult> Handle(GetUserCalendarCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id == 0 ? _currentUserService.UserId : request.Id;
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
                Holidays = (await _dataContext.GetCalendarAsync(request.Year, request.FullYear)).Holidays,
                Summary = await _dataContext.GetAllowanceAsync(id, request.Year),
                LeaveRequested = await _dataContext.Leaves.GetRequested(id, request.Year),
            };
        }
    }
}