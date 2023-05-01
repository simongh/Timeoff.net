using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> NotificationsAsync()
        {
            var result = await _mediator.Send(new Application.Absences.NotificationCommand());

            return Ok(result);
        }
    }
}