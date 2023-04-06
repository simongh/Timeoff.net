using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record DeleteTeamCommand : IRequest<TeamsViewModel>
    {
        public int Id { get; init; }
    }

    internal class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand, TeamsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public DeleteTeamCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<TeamsViewModel> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _dataContext.Teams
                .Where(d => d.TeamId == request.Id && d.CompanyId == _currentUserService.CompanyId)
                .Select(d => new
                {
                    Team = d,
                    Users = d.Users.Count,
                })
                .FirstOrDefaultAsync();

            ResultModels.FlashResult messages;
            if (team == null)
            {
                throw new NotFoundException();
            }
            else if (team.Users > 0)
            {
                messages = ResultModels.FlashResult.WithError($"Team '{team.Team.Name}' cannot be removed as it still has {team.Users} employee(s)");
            }
            else
            {
                _dataContext.Teams.Remove(team.Team);
                await _dataContext.SaveChangesAsync();

                messages = ResultModels.FlashResult.Success($"Team '{team.Team.Name}' was successfully removed");
            }

            var vm = await _dataContext.QueryTeams(_currentUserService.CompanyId);
            vm.Result = messages;

            return vm;
        }
    }
}