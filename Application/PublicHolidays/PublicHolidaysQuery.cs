using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    public record PublicHolidaysQuery : IRequest<IEnumerable<ResultModels.DayResult>>
    {
        public int Year { get; init; } = DateTime.Today.Year;
    }

    internal class PublicHolidaysQueryHandler(IDataContext dataContext)
        : IRequestHandler<PublicHolidaysQuery, IEnumerable<ResultModels.DayResult>>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<ResultModels.DayResult>> Handle(PublicHolidaysQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Calendar
                .Where(h => h.IsHoliday)
                .Where(h => h.Date.Year == request.Year)
                .OrderBy(h => h.Date)
                .ToModel()
                .ToArrayAsync(cancellationToken);
        }
    }
}