using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    public record GetUserCalendarCommand : IRequest<UserCalendarViewModel>
    {
        public int Id { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;
    }

    public class GetUserCalendarComamndHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetUserCalendarCommand, UserCalendarViewModel>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<UserCalendarViewModel> Handle(GetUserCalendarCommand request, CancellationToken cancellationToken)
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
                Id = request.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CurrentYear = request.Year,
                IsActive = user.IsActivated && (user.EndDate == null || user.EndDate > DateTime.Today),
                Calendar = await _dataContext.GetCalendarAsync(_currentUserService.CompanyId, request.Year, true),
                Summary = await _dataContext.GetAllowanceAsync(id, request.Year),
                LeaveRequested = await _dataContext.Leaves.GetRequested(id, request.Year),
            };
        }
    }
}