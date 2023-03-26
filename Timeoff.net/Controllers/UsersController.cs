using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("add")]
        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPostAsync()
        {
            return View();
        }

        [HttpGet("import")]
        public async Task<IActionResult> ImportAsync()
        {
            return View();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportPostAsync()
        {
            return View();
        }

        [HttpGet("import-sample")]
        public async Task<IActionResult> ImportSampleAsync()
        {
            return View();
        }

        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute] Application.Users.GetUserDetailsCommand command)
        {
            var vm = await _mediator.Send(command);

            if (vm == null)
                return RedirectToAction(nameof(IndexAsync));

            return View(vm);
        }

        [HttpGet("edit/{id:int}/absences")]
        public async Task<IActionResult> AbsencesAsync(int id)
        {
            return View();
        }

        [HttpGet("edit/{id:int}/schedule")]
        public async Task<IActionResult> ScheduleAsync(int id)
        {
            return View();
        }

        [HttpGet("edit/{id:int}/calendar")]
        public async Task<IActionResult> CalendarAsync(int id)
        {
            return View();
        }

        [HttpPost("edit/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, Application.Users.UpdateUserCommand command)
        {
            command.Id = id;
            var vm = await _mediator.Send(command);

            return View("edit", vm);
        }

        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return View();
        }

        [HttpGet("search")]
        [HttpPost("search")]
        public async Task<IActionResult> SearchAsync()
        {
            return View();
        }

        [HttpGet("")]
        public async Task<IActionResult> IndexAsync([FromQuery] Application.Users.UsersQuery query)
        {
            return View(await _mediator.Send(query));
        }
    }
}