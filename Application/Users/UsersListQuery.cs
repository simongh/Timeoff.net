using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record UsersListQuery : IRequest<IEnumerable<ResultModels.ListResult>>
    {
    }

    internal class UsersListQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UsersListQuery, IEnumerable<ResultModels.ListResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<ResultModels.ListResult>> Handle(UsersListQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.ListResult
                {
                    Id = u.UserId,
                    Name = u.FirstName + " " + u.LastName,
                })
                .ToArrayAsync();
        }
    }
}