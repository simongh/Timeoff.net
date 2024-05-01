using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record UsersListQuery : IRequest<IEnumerable<ResultModels.ListResult>>
    {
    }

    internal class UsersListQueryHandler(IDataContext dataContext)
        : IRequestHandler<UsersListQuery, IEnumerable<ResultModels.ListResult>>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<ResultModels.ListResult>> Handle(UsersListQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Users
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