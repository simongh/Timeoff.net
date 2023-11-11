using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record GetCalendarCommand : IRequest<CalendarViewModel>
    {
        public int Id { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;
    }

    public class GetCalendarComamndHandler : IRequestHandler<GetCalendarCommand, CalendarViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetCalendarComamndHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<CalendarViewModel> Handle(GetCalendarCommand request, CancellationToken cancellationToken)
        {
            var user = (await _dataContext.Users
                .FindById(request.Id)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.IsActivated,
                    u.EndDate,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync())
                ?? throw new NotFoundException();

            return new()
            {
                Id = request.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CurrentYear = request.Year,
                IsActive = user.IsActivated && (user.EndDate == null || user.EndDate > DateTime.Today),
                Calendar = await _dataContext.GetCalendarAsync(_currentUserService.CompanyId, request.Year, true),
                Summary = await _dataContext.GetAllowanceAsync(request.Id, request.Year),
                LeaveRequested = await _dataContext.Leaves.GetRequested(request.Id, request.Year),
            };
        }
    }
}