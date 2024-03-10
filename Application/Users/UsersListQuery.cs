using MediatR;
using Microsoft.EntityFrameworkCore;
using Timeoff.Services;

namespace Timeoff.Application.Users
{
    public record UsersListQuery : IRequest<IEnumerable<ResultModels.UserResult>>
    {
    }

    internal class UsersListQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UsersListQuery, IEnumerable<ResultModels.UserResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<ResultModels.UserResult>> Handle(UsersListQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.UserResult
                {
                    Id = u.UserId,
                    Name = u.FirstName + " " + u.LastName,
                })
                .ToArrayAsync();
        }
    }
}