using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("reports")]
    public class ReportsController : Controller
    {
        [HttpGet()]
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        [HttpGet("allowancebytime")]
        public async Task<IActionResult> AllowanceByTimeAsync()
        {
            return View();
        }

        [HttpGet("leaves")]
        public async Task<IActionResult> LeavesAsync()
        {
            return View();
        }
    }
}