using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    public record UpdateUserScheduleCommand : IRequest<ScheduleModel>
    {
        public int Id { get; set; }

        public ScheduleModel? Schedule { get; set; }
    }

    internal class UpdateUserScheduleCommandHandler : IRequestHandler<UpdateUserScheduleCommand, ScheduleModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateUserScheduleCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ScheduleModel> Handle(UpdateUserScheduleCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .Where(u => u.UserId == request.Id && u.CompanyId == _currentUserService.CompanyId)
                .AnyAsync();

            if (!user)
            {
                throw new NotFoundException();
            }

            var schedule = await _dataContext.Schedules
                .Where(s => s.UserId == request.Id)
                .FirstOrDefaultAsync();

            if (request.Schedule == null)
            {
                if (schedule != null)
                {
                    _dataContext.Schedules.Remove(schedule);
                }
            }
            else
            {
                if (schedule == null)
                {
                    schedule = new()
                    {
                        UserId = request.Id,
                    };
                    _dataContext.Schedules.Add(schedule);
                }

                schedule.UpdateSchedule(request.Schedule);
            }

            await _dataContext.SaveChangesAsync();

            return await _dataContext.GetUserScheduleModelAsync(_currentUserService.CompanyId, request.Id);
        }
    }
}