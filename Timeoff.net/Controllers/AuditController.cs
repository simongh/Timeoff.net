using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("audit")]
    [Authorize(Roles = "Admin")]
    public class AuditController : Controller
    {
        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("email")]
        public async Task<IActionResult> EmailsAsync([FromQuery] Application.EmailAudit.EmailAuditQuery command)
        {
            return View(await _mediator.Send(command));
        }
    }
}