using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("settings/publicholidays")]
    [Authorize(Roles = "Admin")]
    public class PublicHolidayController : Controller
    {
        private readonly IMediator _mediator;

        public PublicHolidayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> IndexAsync([FromQuery] Application.PublicHoliday.PublicHolidaysQuery query)
        {
            var vm = await _mediator.Send(query);
            return View(vm);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(Application.PublicHoliday.UpdatePublicHolidayCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportAsync()
        {
            return View();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(Application.PublicHoliday.DeleteHolidayCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("index", vm);
        }
    }
}