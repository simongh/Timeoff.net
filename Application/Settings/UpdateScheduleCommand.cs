﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record UpdateScheduleCommand : Types.ScheduleModel, IRequest<SettingsViewModel>
    {
    }

    internal class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, SettingsViewModel>
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

        public async Task<SettingsViewModel> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
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