using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("list")]
        public async Task<IActionResult> GetUserListAsync()
        {
            return Ok(await _mediator.Send(new Application.Users.UsersListQuery()));
        }

        [HttpGet]
        public Task<IActionResult> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> CreateAsync()
        {
            throw new NotImplementedException();
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