using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("users")]
    [Authorize(Roles = "Admin")]
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
            return View(await _mediator.Send(new Application.Users.GetCreateCommand()));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPostAsync(Application.Users.CreateCommand command)
        {
            var vm = await _mediator.Send(command);
            if (vm.Messages!.Errors == null)
            {
                var usersVm = await _mediator.Send(new Application.Users.UsersQuery());
                return View("Index", usersVm);
            }

            return View("Create", vm);
        }

        [HttpGet("import")]
        public IActionResult Import()
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
        public async Task<IActionResult> EditAsync([FromRoute] Application.Users.GetDetailsCommand command)
        {
            var vm = await _mediator.Send(command);

            if (vm == null)
                return RedirectToAction(nameof(IndexAsync));

            return View(vm);
        }

        [HttpGet("edit/{id:int}/absences")]
        public async Task<IActionResult> AbsencesAsync(Application.Users.GetAbsencesCommand command)
        {
            var vm = await _mediator.Send(command);

            return View(vm);
        }

        [HttpGet("edit/{id:int}/schedule")]
        public async Task<IActionResult> ScheduleAsync([FromRoute] Application.Users.GetScheduleCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
        }

        [HttpPost("edit/{id:int}/schedule")]
        public async Task<IActionResult> SetScheduleAsync(int id, Application.Users.UpdateScheduleCommand command)
        {
            command.Id = id;
            var vm = await _mediator.Send(command);

            return View("Schedule", vm);
        }

        [HttpGet("edit/{id:int}/calendar")]
        public async Task<IActionResult> CalendarAsync(Application.Users.GetCalendarCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
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

        [HttpPost("edit/reset-password/{id:int}")]
        public async Task<IActionResult> ResetPasswordAsync([FromRoute] Application.Users.ResetPasswordCommand command)
        {
            var vm = await _mediator.Send(command);

            if (vm == null)
            {
                return RedirectToAction(nameof(IndexAsync));
            }

            return View("Edit", vm);
        }
    }
}