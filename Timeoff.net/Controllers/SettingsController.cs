using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Timeoff.Application.Schedule;

namespace Timeoff.Controllers
{
    [Route("settings")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet("")]
        //public async Task<IActionResult> IndexAsync()
        //{
        //    return View(await _mediator.Send(new Application.Settings.GetSettingsCommand()));
        //}

        [HttpPost("company")]
        public async Task<IActionResult> UpdateCompanyAsync(Application.Settings.UpdateSettingsCommand command)
        {
            return View("index", await _mediator.Send(command));
        }

        [HttpPost("carryOverUnusedAllowance")]
        public async Task<IActionResult> CarryOverAsync()
        {
            return View();
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleAsync(UpdateScheduleCommand command)
        {
            return View("index", await _mediator.Send(command));
        }

        [HttpPost("leavetypes")]
        public async Task<IActionResult> UpdateLeaveTypesAsync(Application.Settings.UpdateLeaveTypesCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("index", vm);
        }

        [HttpPost("leavetypes/delete")]
        public async Task<IActionResult> DeleteLeaveTypesAsync([FromQuery] Application.Settings.DeleteLeaveTypeCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("general", vm);
        }

        [HttpGet("company/authentication")]
        public async Task<IActionResult> CompanyAuthenticateAsync()
        {
            return View();
        }

        [HttpPost("company/authentication")]
        public async Task<IActionResult> UpdateCompanyAuthenticateAsync()
        {
            return View();
        }

        [HttpGet("company/backup")]
        public async Task<IActionResult> CompanyBackupAsync()
        {
            return View();
        }

        [HttpPost("company/delete")]
        public async Task<IActionResult> CompanyDeleteAsync(Application.DeleteCompany.DeleteCompanyCommand command)
        {
            var vm = await _mediator.Send(command);

            if (vm != null)
            {
                return View("index", vm);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }
    }
}