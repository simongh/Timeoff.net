using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/company/leave-types")]
    [ApiController]
    [Authorize(Policy = "token", Roles = Roles.Admin)]
    public class LeaveTypesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeleteLeaveType.DeleteLeaveTypeCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            else
                return BadRequest(result);
        }
    }
}