using MediatR;

namespace Timeoff.Application.PublicHoliday
{
    public record PublicHolidaysQuery : IRequest<PublicHolidaysViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;
    }

    internal class PublicHolidaysQueryHandler : IRequestHandler<PublicHolidaysQuery, PublicHolidaysViewModel>
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

        public async Task<PublicHolidaysViewModel> Handle(PublicHolidaysQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Companies.GetPublicHolidaysAsync(
                _currentUserService.CompanyId,
                request.Year);
        }
    }
}