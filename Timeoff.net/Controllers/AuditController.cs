using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("")]
    public class AuditController : Controller
    {
        [HttpGet("email")]
        public async Task<IActionResult> EmailAsync()
        {
            return View();
        }
    }
}