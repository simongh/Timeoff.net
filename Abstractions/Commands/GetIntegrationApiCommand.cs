using MediatR;

namespace Timeoff.Commands
{
    public record GetIntegrationApiCommand : IRequest<ResultModels.IntegrationApiViewModel>
    {
    }

    internal class GetIntegrationApiCommandHandler : IRequestHandler<GetIntegrationApiCommand, ResultModels.IntegrationApiViewModel>
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

        public async Task<ResultModels.IntegrationApiViewModel> Handle(GetIntegrationApiCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetIntegrationApiAsync(_currentUserService.CompanyId);
        }
    }
}