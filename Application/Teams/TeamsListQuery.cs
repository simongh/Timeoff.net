using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record TeamsListQuery : IRequest<IEnumerable<ResultModels.ListResult>>
    { }

    internal class TeamsListQueryHandler(IDataContext dataContext)
        : IRequestHandler<TeamsListQuery, IEnumerable<ResultModels.ListResult>>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<ResultModels.ListResult>> Handle(TeamsListQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Teams
                .OrderBy(t => t.Name)
                .Select(t => new ResultModels.ListResult
                {
                    Id = t.TeamId,
                    Name = t.Name,
                })
                .ToArrayAsync();
        }
    }
}