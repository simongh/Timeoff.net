using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "token")]
    public class CalendarController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("")]
        public async Task<IActionResult> GetAsync([FromQuery] Application.Calendar.CalendarQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("teams")]
        public async Task<IActionResult> TeamsAsync([FromQuery] Application.TeamView.TeamViewQuery query)
        {
            var results = await _mediator.Send(query);

            return Ok(results);
        }
    }
}