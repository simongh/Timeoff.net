using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    public record UpdateScheduleCommand : ScheduleModel, IRequest<Settings.SettingsViewModel>
    {
    }

    internal class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, Settings.SettingsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateScheduleCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<Settings.SettingsViewModel> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _dataContext.Schedules
                .Where(s => s.CompanyId == _currentUserService.CompanyId)
                .FirstOrDefaultAsync();

            if (schedule == null)
            {
                throw new NotFoundException();
            }

            schedule.UpdateSchedule(request);

            await _dataContext.SaveChangesAsync();

            var result = await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
            result.Result = ResultModels.FlashResult.Success("Schedule updated");

            return result;
        }
    }
}