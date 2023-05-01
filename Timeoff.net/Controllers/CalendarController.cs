using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("calendar")]
    public class CalendarController : Controller
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("bookleave")]
        public async Task<IActionResult> BookAsync(Application.Absences.BookCommand command)
        {
            await _mediator.Send(command);

            var result = _mediator.Send(new Commands.GetCalendarCommand());

            return View("Index", result);
        }

        [HttpGet]
        [Route("~/")]
        public async Task<IActionResult> IndexAsync([FromQuery] Commands.GetCalendarCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
        }

        [HttpGet("teamview")]
        public async Task<IActionResult> TeamViewAsync()
        {
            return View();
        }

        [HttpGet("feeds")]
        public async Task<IActionResult> FeedsAsync()
        {
            return View();
        }

        [HttpPost("feeds/regenerate")]
        public async Task<IActionResult> RegenerateAsync()
        {
            return View();
        }

        [HttpGet("leave-summary/{int:int}")]
        public async Task<IActionResult> SummaryAsync(int id)
        {
            return View();
        }
    }
}