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

        [HttpGet("general")]
        public async Task<IActionResult> GeneralAsync()
        {
            return View(await _mediator.Send(new Commands.GetSettingsCommand()));
        }

        [HttpPost("company")]
        public async Task<IActionResult> UpdateCompanyAsync(Commands.UpdateSettingsCommand command)
        {
            return View("general", await _mediator.Send(command));
        }

        [HttpPost("carryOverUnusedAllowance")]
        public async Task<IActionResult> CarryOverAsync()
        {
            return View();
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleAsync()
        {
            return View();
        }

        [HttpPost("leavetypes")]
        public async Task<IActionResult> UpdateLeaveTypesAsync(Commands.UpdateLeaveTypesCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("general", vm);
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
            return View();
        }

        [HttpPost("company/integration-api")]
        public async Task<IActionResult> UpdateCompanyIntegrationApiAsync()
        {
            return View();
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
                return View("general", vm);
            }
            else
            {
                return RedirectToAction("Logout", "Account");
            }
        }
    }
}