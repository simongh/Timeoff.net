using MediatR;

namespace Timeoff.Application.CreateUser
{
    public record GetCreateCommand : IRequest<CreateViewModel>
    {
    }

    internal class GetCreateCommandHandler : IRequestHandler<GetCreateCommand, CreateViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetCreateCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<CreateViewModel> Handle(GetCreateCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetCreateViewModelAsync(_currentUserService.CompanyId);
        }
    }
}