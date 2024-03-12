using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record TeamsListQuery : IRequest<IEnumerable<ResultModels.ListResult>>
    { }

    internal class TeamsListQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<TeamsListQuery, IEnumerable<ResultModels.ListResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<ResultModels.ListResult>> Handle(TeamsListQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.Teams
                .Where(t => t.CompanyId == _currentUserService.CompanyId)
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