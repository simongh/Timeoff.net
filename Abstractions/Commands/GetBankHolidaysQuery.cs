using MediatR;

namespace Timeoff.Commands
{
    public record GetBankHolidaysQuery : IRequest<ResultModels.PublicHolidaysViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;
    }

    internal class GetBankHolidaysQueryHandler : IRequestHandler<GetBankHolidaysQuery, ResultModels.PublicHolidaysViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetBankHolidaysQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.PublicHolidaysViewModel> Handle(GetBankHolidaysQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Companies.GetPublicHolidaysAsync(
                _currentUserService.CompanyId,
                request.Year);
        }
    }
}