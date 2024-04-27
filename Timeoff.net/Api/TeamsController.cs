using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "token", Roles = Roles.Admin)]
    public class TeamsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            return Ok(await _mediator.Send(new Application.Teams.TeamsQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Application.Teams.UpdateTeamCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] Application.Teams.GetTeamCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, Application.Teams.UpdateTeamCommand command)
        {
            if (command == null)
                return BadRequest();

            command.Id = id;

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeleteTeam.DeleteTeamCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}