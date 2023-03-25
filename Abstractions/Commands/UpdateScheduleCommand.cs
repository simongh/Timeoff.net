using MediatR;

namespace Timeoff.Commands
{
    public record UpdateScheduleCommand : IRequest<ResultModels.SettingsViewModel>
    {
        public bool Monday { get; init; }
        public bool Tuesday { get; init; }
        public bool Wednesday { get; init; }
        public bool Thursday { get; init; }
        public bool Friday { get; init; }
        public bool Saturday { get; init; }
        public bool Sunday { get; init; }
    }

    internal class UpdateScheduleCommandHandler : IRequestHandler<UpdateScheduleCommand, ResultModels.SettingsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateScheduleCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.SettingsViewModel> Handle(UpdateScheduleCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
        }
    }
}