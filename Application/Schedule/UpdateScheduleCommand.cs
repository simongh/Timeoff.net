using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    public record UpdateScheduleCommand : Types.ScheduleModel, IRequest<ResultModels.ApiResult>
    {
    }

    internal class UpdateScheduleCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService) : IRequestHandler<UpdateScheduleCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<ResultModels.ApiResult> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
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

            return new();
        }
    }
}