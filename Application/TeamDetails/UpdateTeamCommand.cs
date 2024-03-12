using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.TeamDetails
{
    public record UpdateTeamCommand : Types.TeamModel, IRequest<Teams.TeamsViewModel?>, Commands.IValidated
    {
        public int? Id { get; set; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Teams.TeamsViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateTeamCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<Teams.TeamsViewModel?> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult messages;

            if (request.Failures.IsValid())
            {
                var managerFound = await _dataContext.Users
                    .Where(u => u.CompanyId == _currentUserService.CompanyId)
                    .Where(u => u.UserId == request.ManagerId)
                    .AnyAsync();

                if (!managerFound)
                {
                    return new()
                    {
                        Result = ResultModels.FlashResult.WithError("Invalid manager"),
                    };
                }

                Entities.Team? team;
                if (request.Id == null)
                {
                    team = new()
                    {
                        CompanyId = _currentUserService.CompanyId,
                    };
                    _dataContext.Teams.Add(team);
                }
                else
                {
                    team = await _dataContext.Teams
                        .Where(d => d.TeamId == request.Id.Value && d.CompanyId == _currentUserService.CompanyId)
                        .FirstOrDefaultAsync();

                    if (team == null)
                        return new()
                        {
                            Result = ResultModels.FlashResult.WithError("Invalid team"),
                        };
                }

                team.Name = request.Name;
                team.Allowance = request.Allowance;
                team.IsAccrued = request.IsAccruedAllowance;
                team.IncludePublicHolidays = request.IncludePublicHolidays;
                team.ManagerId = request.ManagerId;

                await _dataContext.SaveChangesAsync();

                if (request.Id == null)
                    messages = ResultModels.FlashResult.Success("Team added");
                else
                    messages = ResultModels.FlashResult.Success($"Team {team.Name} was updated");
            }
            else
            {
                messages = request.Failures.ToFlashResult();
            }

            return new()
            {
                Result = messages,
            };
        }
    }
}