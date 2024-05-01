using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    public record UpdateScheduleCommand : Types.ScheduleModel, IRequest<ResultModels.ApiResult>
    {
    }

    internal class UpdateScheduleCommandHandler(IDataContext dataContext)
        : IRequestHandler<UpdateScheduleCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<ResultModels.ApiResult> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            var schedule = await _dataContext.Schedules
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