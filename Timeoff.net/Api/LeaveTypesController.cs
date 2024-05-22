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
        public async Task<IActionResult> UpdateAsync(Application.Settings.UpdateLeaveTypesCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(Application.Settings.LeaveTypeRequest command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new Application.Settings.UpdateLeaveTypesCommand
            {
                Add = command
            });

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
                return BadRequest(result);
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