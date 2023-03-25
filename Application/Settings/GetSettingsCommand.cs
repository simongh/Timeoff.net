using MediatR;

namespace Timeoff.Application.Settings
{
    public record GetSettingsCommand : IRequest<SettingsViewModel>
    {
    }

    internal class GetSettingsCommandHandler : IRequestHandler<GetSettingsCommand, SettingsViewModel>
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

        public async Task<SettingsViewModel> Handle(GetSettingsCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
        }
    }
}