using MediatR;

namespace Timeoff.Application.Schedule
{
    public record GetScheduleCommand : IRequest<ScheduleViewModel>
    {
        public int Id { get; set; }
    }

    internal class GetUserScheduleCommandHandler : IRequestHandler<GetScheduleCommand, ScheduleViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetUserScheduleCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ScheduleViewModel> Handle(GetScheduleCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetUserScheduleAsync(_currentUserService.CompanyId, request.Id);
        }
    }
}