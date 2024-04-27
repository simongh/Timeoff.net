using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "token", Roles = Roles.Admin)]
    public class AuditController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("email")]
        public async Task<IActionResult> GetEmailsAsync([FromQuery] Application.EmailAudit.EmailAuditQuery command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}