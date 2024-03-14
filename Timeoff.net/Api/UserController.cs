using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] Application.Users.UsersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeamsAsync()
        {
            return Ok(await _mediator.Send(new Application.Teams.TeamsListQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Application.CreateUser.CreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Messages?.Errors?.Any() == true)
                return BadRequest(result.Messages);
            else
                return NoContent();
        }

        [HttpGet("{id:int}")]
        public Task<IActionResult> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public Task<IActionResult> UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}