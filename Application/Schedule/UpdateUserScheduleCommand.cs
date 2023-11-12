using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    public record UpdateUserScheduleCommand : ScheduleModel, IRequest<ScheduleViewModel>
    {
        public int Id { get; set; }

        public string? Action { get; init; }
    }

    internal class UpdateUserScheduleCommandHandler : IRequestHandler<UpdateUserScheduleCommand, ScheduleViewModel>
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

        public async Task<ScheduleViewModel> Handle(UpdateUserScheduleCommand request, CancellationToken cancellationToken)
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

            if (request.Action == "reset")
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

                schedule.UpdateSchedule(request);
            }

            await _dataContext.SaveChangesAsync();

            var result = await _dataContext.GetUserScheduleAsync(_currentUserService.CompanyId, request.Id);
            result.Messages = ResultModels.FlashResult.Success("Schedule updated");

            return result;
        }
    }
}