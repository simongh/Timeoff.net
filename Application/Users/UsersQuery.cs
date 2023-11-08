using MediatR;

namespace Timeoff.Application.Users
{
    public record UsersQuery : IRequest<UsersViewModel>
    {
        public int? Team { get; init; }

        public bool AsCsv { get; init; }
    }

    internal class UsersQueryHandler : IRequestHandler<UsersQuery, UsersViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UsersQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<UsersViewModel> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.QueryUsers(_currentUserService.CompanyId, request.Team);
        }
    }
}