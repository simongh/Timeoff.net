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

        //[HttpPost("bookleave")]
        //public async Task<IActionResult> BookAsync(Application.BookAbsence.BookCommand command)
        //{
        //    await _mediator.Send(command);

        //    var result = _mediator.Send(new Application.Calendar.GetCalendarCommand());

        //    return View("Index", result);
        //}

        //[HttpGet]
        //[Route("~/")]
        //public async Task<IActionResult> IndexAsync([FromQuery] Application.Calendar.GetCalendarCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    return View(vm);
        //}

        [HttpGet("teamview")]
        public async Task<IActionResult> TeamViewAsync([FromQuery] Application.TeamView.GetTeamViewCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
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

        [HttpGet("leave-summary/{id:int}")]
        public async Task<IActionResult> SummaryAsync([FromRoute] Application.LeaveSummary.SummaryCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
        }
    }
}