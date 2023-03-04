using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("audit")]
    public class AuditController : Controller
    {
        [HttpGet("email")]
        public async Task<IActionResult> EmailAsync()
        {
            return View();
        }
    }
}