using MediatR;

namespace Timeoff.Application.IntegrationApi
{
    public record GetIntegrationApiCommand : IRequest<IntegrationResult>
    {
    }

    internal class GetIntegrationApiCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetIntegrationApiCommand, IntegrationResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IntegrationResult> Handle(GetIntegrationApiCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetIntegrationApiAsync(_currentUserService.CompanyId);
        }
    }
}