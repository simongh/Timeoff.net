using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        //[HttpGet("email")]
        //public async Task<IActionResult> GetEmailsAsync([FromQuery] Application.EmailAudit.EmailAuditQuery command)
        //{
        //    return Ok(await _mediator.Send(command));
        //}
    }
}