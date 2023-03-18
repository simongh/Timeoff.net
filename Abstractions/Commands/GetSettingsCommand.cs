using MediatR;

namespace Timeoff.Commands
{
    public record GetSettingsCommand : IRequest<ResultModels.SettingsViewModel>
    {
    }

    internal class GetSettingsCommandHandler : IRequestHandler<GetSettingsCommand, ResultModels.SettingsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetSettingsCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.SettingsViewModel> Handle(GetSettingsCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
        }
    }
}