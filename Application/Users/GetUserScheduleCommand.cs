using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record GetUserScheduleCommand : IRequest<UserScheduleViewModel>
    {
        public int Id { get; set; }
    }

    internal class GetUserScheduleCommandHandler : IRequestHandler<GetUserScheduleCommand, UserScheduleViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetUserScheduleCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<UserScheduleViewModel?> Handle(GetUserScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _dataContext.Users
                 .Where(u => u.CompanyId == _currentUserService.CompanyId && u.UserId == request.Id)
                 .Select(u => new
                 {
                     User = u.Schedule,
                     Company = u.Company.Schedule,
                     u.FirstName,
                     u.LastName,
                     u.IsActivated,
                 })
                 .FirstOrDefaultAsync();

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            return new()
            {
                Schedule = (schedule.User ?? schedule.Company).ToEnumerable(),
                Id = request.Id,
                FirstName = schedule.FirstName,
                LastName = schedule.LastName,
                IsActive = schedule.IsActivated,
                UserSpecific = schedule.User != null,
            };
        }
    }
}