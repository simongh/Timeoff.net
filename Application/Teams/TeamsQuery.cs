using MediatR;

namespace Timeoff.Application.Teams
{
    public record TeamsQuery : IRequest<IEnumerable<TeamResult>>
    {
    }

    internal class TeamsQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<TeamsQuery, IEnumerable<TeamResult>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<IEnumerable<TeamResult>> Handle(TeamsQuery request, CancellationToken cancellationToken)
        {
            var result = await _dataContext.QueryTeams(_currentUserService.CompanyId);

            return result.Teams;
        }
    }
}