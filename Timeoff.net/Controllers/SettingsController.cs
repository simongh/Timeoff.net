using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("")]
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _mediator.Send(new Commands.GetSettingsCommand()));
        }

        [HttpPost("company")]
        public async Task<IActionResult> UpdateCompanyAsync(Commands.UpdateSettingsCommand command)
        {
            return View("index", await _mediator.Send(command));
        }

        [HttpPost("carryOverUnusedAllowance")]
        public async Task<IActionResult> CarryOverAsync()
        {
            return View();
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleAsync(Commands.UpdateScheduleCommand command)
        {
            return View("index", await _mediator.Send(command));
        }

        [HttpPost("leavetypes")]
        public async Task<IActionResult> UpdateLeaveTypesAsync(Commands.UpdateLeaveTypesCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("index", vm);
        }

        [HttpPost("leavetypes/delete")]
        public async Task<IActionResult> DeleteLeaveTypesAsync([FromQuery] Commands.DeleteLeaveTypeCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("general", vm);
        }

        [HttpGet("company/integration-api")]
        public async Task<IActionResult> CompanyIntegrationApiAsync()
        {
            return View(await _mediator.Send(new Commands.GetIntegrationApiCommand()));
        }

        [HttpPost("company/integration-api")]
        public async Task<IActionResult> UpdateCompanyIntegrationApiAsync(Commands.UpdateIntegrationApiCommand command)
        {
            return View("CompanyIntegrationApi", await _mediator.Send(command));
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
        public async Task<IActionResult> CompanyDeleteAsync(Commands.DeleteCompanyCommand command)
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