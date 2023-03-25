using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record UpdateIntegrationApiCommand : IRequest<IntegrationApiViewModel>
    {
        public bool Enabled { get; init; }

        public bool Regenerate { get; init; }
    }

    internal class UpdateIntegrationApiCommandHandler : IRequestHandler<UpdateIntegrationApiCommand, IntegrationApiViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateIntegrationApiCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<IntegrationApiViewModel> Handle(UpdateIntegrationApiCommand request, CancellationToken cancellationToken)
        {
            var company = await _dataContext.Companies
                .Where(c => c.CompanyId == _currentUserService.CompanyId)
                .FirstAsync();

            if (request.Regenerate)
            {
                company.IntegrationApiToken = Guid.NewGuid();
            }
            company.IntegrationApiEnabled = request.Enabled;

            await _dataContext.SaveChangesAsync();

            var result = await _dataContext.GetIntegrationApiAsync(_currentUserService.CompanyId);
            result.Messages = ResultModels.FlashResult.Success("Settings were saved");

            return result;
        }
    }
}