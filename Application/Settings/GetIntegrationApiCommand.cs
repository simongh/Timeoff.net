using MediatR;

namespace Timeoff.Application.Settings
{
    public record GetIntegrationApiCommand : IRequest<IntegrationApiViewModel>
    {
    }

    internal class GetIntegrationApiCommandHandler : IRequestHandler<GetIntegrationApiCommand, IntegrationApiViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetIntegrationApiCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<IntegrationApiViewModel> Handle(GetIntegrationApiCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetIntegrationApiAsync(_currentUserService.CompanyId);
        }
    }
}