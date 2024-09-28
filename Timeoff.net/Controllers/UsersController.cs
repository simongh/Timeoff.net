using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("users")]
    [Authorize(Roles = "Admin")]
    public class UsersController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        //[HttpGet("add")]
        //public async Task<IActionResult> CreateAsync()
        //{
        //    return View(await _mediator.Send(new Application.CreateUser.GetCreateCommand()));
        //}

        //[HttpPost("add")]
        //public async Task<IActionResult> AddPostAsync(Application.CreateUser.CreateCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    if (vm.Messages!.Errors == null)
        //    {
        //        var usersVm = await _mediator.Send(new Application.Users.UsersQuery());
        //        return View("Index", usersVm);
        //    }

        //    return View("Create", vm);
        //}

        //[HttpGet("import")]
        //public IActionResult Import()
        //{
        //    return View();
        //}

        //[HttpPost("import")]
        //public async Task<IActionResult> ImportPostAsync()
        //{
        //    return View();
        //}

        [HttpGet("import-sample")]
        public async Task<IActionResult> ImportSampleAsync()
        {
            var result = await _mediator.Send(new Application.Users.SampleCommand());

            return File(result, "text/csv", "sample.csv");
        }

        //[HttpGet("edit/{id:int}")]
        //public async Task<IActionResult> EditAsync([FromRoute] Application.UserDetails.GetDetailsCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    //if (vm == null)
        //    //    return RedirectToAction(nameof(IndexAsync));

        //    return View(vm);
        //}

        //[HttpGet("edit/{id:int}/absences")]
        //public async Task<IActionResult> AbsencesAsync(Application.Absences.GetAbsencesCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    return View(vm);
        //}

        //[HttpPost("edit/{id:int}/absences")]
        //public async Task<IActionResult> UpdateAbsencesAsync(Application.Absences.UpdateAbsencesCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    return View("Absences", vm);
        //}

        //[HttpGet("edit/{id:int}/schedule")]
        //public async Task<IActionResult> ScheduleAsync([FromRoute] Application.Schedule.GetScheduleCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    return View(vm);
        //}

        //[HttpPost("edit/{id:int}/schedule")]
        //public async Task<IActionResult> SetScheduleAsync(Application.Schedule.UpdateUserScheduleCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    return View("Schedule", vm);
        //}

        //[HttpGet("edit/{id:int}/calendar")]
        //public async Task<IActionResult> CalendarAsync(Application.Calendar.GetUserCalendarCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    return View(vm);
        //}

        //[HttpPost("edit/{id:int}")]
        //public async Task<IActionResult> UpdateAsync(Application.UserDetails.UpdateUserCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    return View("edit", vm);
        //}

        //[HttpPost("delete/{id:int}")]
        //public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeleteUser.DeleteUserCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    return View("Index", vm);
        //}

        //[HttpGet("search")]
        //[HttpPost("search")]
        //public async Task<IActionResult> SearchAsync()
        //{
        //    return View();
        //}

        //[HttpGet("")]
        //public async Task<IActionResult> IndexAsync([FromQuery] Application.Users.UsersQuery query)
        //{
        //    return View(await _mediator.Send(query));
        //}

        //[HttpPost("edit/reset-password/{id:int}")]
        //public async Task<IActionResult> ResetPasswordAsync([FromRoute] Application.ResetPassword.ResetUserPasswordCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    if (vm == null)
        //    {
        //        //return RedirectToAction(nameof(IndexAsync));
        //    }

        //    return View("Edit", vm);
        //}

        //[HttpGet("summary/{id:int}")]
        //[Authorize()]
        //public async Task<IActionResult> SummaryAsync([FromRoute] Application.UserSummary.SummaryCommand command)
        //{
        //    var vm = await _mediator.Send(command);

        //    return View(vm);
        //}
    }
}