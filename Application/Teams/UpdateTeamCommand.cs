using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Teams
{
    public record UpdateTeamCommand : Types.TeamModel, IRequest<TeamsViewModel?>, Commands.IValidated
    {
        public int? Id { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class NewDepartmentCommandHandler : IRequestHandler<UpdateTeamCommand, TeamsViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public NewDepartmentCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<TeamsViewModel?> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult messages;

            if (request.Failures.IsValid())
            {
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
                        .Where(d => d.DepartmentId == request.Id.Value && d.CompanyId == _currentUserService.CompanyId)
                        .FirstOrDefaultAsync();

                    if (team == null)
                        return null;
                }

                team.Name = request.Name;
                team.Allowance = request.Allowance;
                team.IsAccrued = request.IsAccruedAllowance;
                team.IncludeBankHolidays = request.IncludePublicHolidays;
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

            var result = await _dataContext.QueryDepartments(_currentUserService.CompanyId);
            result.Result = messages;
            return result;
        }
    }
}