using MediatR;

namespace Timeoff.Application.PublicHolidays
{
    public record PublicHolidaysQuery : IRequest<IEnumerable<ResultModels.PublicHolidayResult>>
    {
        public int Year { get; init; } = DateTime.Today.Year;
    }

    internal class PublicHolidaysQueryHandler : IRequestHandler<PublicHolidaysQuery, IEnumerable<ResultModels.PublicHolidayResult>>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public PublicHolidaysQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ResultModels.PublicHolidayResult>> Handle(PublicHolidaysQuery request, CancellationToken cancellationToken)
        {
            return (await _dataContext.Companies.GetPublicHolidaysAsync(
                _currentUserService.CompanyId,
                request.Year)).PublicHolidays;
        }
    }
}