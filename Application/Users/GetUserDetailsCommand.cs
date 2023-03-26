using MediatR;

namespace Timeoff.Application.Users
{
    public record GetUserDetailsCommand : IRequest<UserDetailsViewModel>
    {
        public int Id { get; init; }
    }

    internal class GetUserDetailsCommandHandler : IRequestHandler<GetUserDetailsCommand, UserDetailsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetUserDetailsCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<UserDetailsViewModel> Handle(GetUserDetailsCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.GetUserDetailsAsync(_currentUserService.CompanyId, request.Id);
        }
    }
}