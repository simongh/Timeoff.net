using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            return Ok(await _mediator.Send(new Application.Teams.TeamsQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Application.TeamDetails.UpdateTeamCommand command)
        {
            if (command == null)
                return BadRequest();

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] Application.TeamDetails.GetTeamCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, Application.TeamDetails.UpdateTeamCommand command)
        {
            if (command == null)
                return BadRequest();

            command.Id = id;

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeleteTeam.DeleteTeamCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}