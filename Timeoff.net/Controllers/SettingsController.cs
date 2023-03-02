using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    public class SettingsController : Controller
    {
        [HttpGet("general")]
        public async Task<IActionResult> GeneralAsync()
        {
            return View();
        }

        [HttpPost("company")]
        public async Task<IActionResult> NewCompanyAsync()
        {
            return View();
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
        public async Task<IActionResult> CreateLeaveTypesAsync()
        {
            return View();
        }

        [HttpPost("leavetypes/delete/{id:int}")]
        public async Task<IActionResult> DeleteLeaveTypesAsync(int id)
        {
            return View();
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
        public async Task<IActionResult> CompanyDeleteAsync()
        {
            return View();
        }
    }
}