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
        public async Task<IActionResult> GetAsync(int? user, int year)
        {
            return Ok(await _mediator.Send(new Application.Calendar.GetUserCalendarCommand
            {
                Id = user ?? 0,
                Year = year,
            }));
        }
    }
}