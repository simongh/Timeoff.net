using MediatR;

namespace Timeoff.Application.IntegrationApi
{
    public record GetIntegrationApiCommand : IRequest<IntegrationResult>
    {
    }

    internal class GetIntegrationApiCommandHandler : IRequestHandler<GetIntegrationApiCommand, IntegrationResult>
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

        public async Task<IntegrationResult> Handle(GetIntegrationApiCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetIntegrationApiAsync(_currentUserService.CompanyId);
        }
    }
}