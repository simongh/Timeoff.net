using MediatR;

namespace Timeoff.Application.Users
{
    public record UsersQuery : IRequest<IEnumerable<UserInfoResult>>
    {
        public int? Team { get; init; }

        public bool AsCsv { get; init; }
    }

    internal class UsersQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UsersQuery, IEnumerable<UserInfoResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<UserInfoResult>> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _dataContext.QueryUsers(_currentUserService.CompanyId, request.Team);

            return result.Users;
        }
    }
}