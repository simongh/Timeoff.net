﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Schedule
{
    public record UpdateUserScheduleCommand : IRequest<Types.ScheduleModel>
    {
        public int Id { get; set; }

        public Types.ScheduleModel? Schedule { get; set; }
    }

    internal class UpdateUserScheduleCommandHandler(IDataContext dataContext)
        : IRequestHandler<UpdateUserScheduleCommand, Types.ScheduleModel>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<Types.ScheduleModel> Handle(UpdateUserScheduleCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .Where(u => u.UserId == request.Id)
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

            return await _dataContext.GetUserScheduleModelAsync(request.Id);
        }
    }
}