using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("integration/v1")]
    [ApiController]
    public class IntegrationController : ApiController
    {
        [HttpGet("report/allowance")]
        public async Task<IActionResult> AllowanceReportAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("report/absence")]
        public async Task<IActionResult> AbsenceReportAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("audit")]
        public async Task<IActionResult> AuditReportAsync()
        {
            throw new NotImplementedException();
        }
    }
}