﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.DeleteTeam
{
    public record DeleteTeamCommand : IRequest
    {
        public int Id { get; init; }
    }

    internal class DeleteTeamCommandHandler(IDataContext dataContext)
        : IRequestHandler<DeleteTeamCommand>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _dataContext.Teams
                .Where(d => d.TeamId == request.Id)
                .Select(d => new
                {
                    Team = d,
                    Users = d.Users.Count,
                })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                throw new NotFoundException();
            }
            else if (team.Users > 0)
            {
                throw new ValidationException("", $"Team '{team.Team.Name}' cannot be removed as it still has {team.Users} employee(s)");
            }
            else
            {
                _dataContext.Teams.Remove(team.Team);
                await _dataContext.SaveChangesAsync();

                //messages = ResultModels.FlashResult.Success($"Team '{team.Team.Name}' was successfully removed");
            }
        }
    }
}