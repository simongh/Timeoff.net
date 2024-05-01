using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    public record PublicHolidaysQuery : IRequest<IEnumerable<ResultModels.PublicHolidayResult>>
    {
        public int Year { get; init; } = DateTime.Today.Year;
    }

    internal class PublicHolidaysQueryHandler(IDataContext dataContext)
        : IRequestHandler<PublicHolidaysQuery, IEnumerable<ResultModels.PublicHolidayResult>>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<ResultModels.PublicHolidayResult>> Handle(PublicHolidaysQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Calendar
                .Where(h => h.IsHoliday)
                .Where(h => h.Date.Year == request.Year)
                .OrderBy(h => h.Date)
                .Select(h => new ResultModels.PublicHolidayResult
                {
                    Date = h.Date,
                    Name = h.Name!,
                    Id = h.CalendarId,
                })
                .ToArrayAsync();
        }
    }
}