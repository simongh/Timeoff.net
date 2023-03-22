using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("audit")]
    public class AuditController : Controller
    {
        private readonly IMediator _mediator;

        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("email")]
        public async Task<IActionResult> EmailAsync([FromQuery] Commands.EmailAuditQuery command)
        {
            return View(await _mediator.Send(command));
        }
    }
}