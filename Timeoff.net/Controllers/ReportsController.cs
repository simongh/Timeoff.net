using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("reports")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        //[HttpGet("")]
        //public IActionResult Index()
        //{
        //    return View();
        //}

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