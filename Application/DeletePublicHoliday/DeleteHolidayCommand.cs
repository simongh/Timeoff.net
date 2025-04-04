﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.DeletePublicHoliday
{
    public record DeleteHolidayCommand : IRequest<ResultModels.ApiResult>
    {
        public int Id { get; init; }
    }

    internal class DeleteHolidayCommandHandler(
        IDataContext dataContext,
        Services.IDaysCalculator adjuster)
        : IRequestHandler<DeleteHolidayCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.IDaysCalculator _adjuster = adjuster;

        public async Task<ResultModels.ApiResult> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            var holiday = await _dataContext.Calendar
                .Where(h => h.CalendarId == request.Id)
                .FirstOrDefaultAsync();

            if (holiday == null)
            {
                return new()
                {
                    Errors = ["Unabled to find holiday"]
                };
            }
            else
            {
                _dataContext.Calendar.Remove(holiday);
                await _dataContext.SaveChangesAsync();

                await _adjuster.AdjustForHolidaysAsync(new[] { (holiday.Date, holiday.Date) });
            }

            return new();
        }
    }
}