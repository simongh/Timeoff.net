using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("calendar")]
    public class CalendarController : Controller
    {
        [HttpPost("bookleave")]
        public async Task<IActionResult> BookAsync()
        {
            return View();
        }

        [HttpGet]
        [Route("~/")]
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        [HttpGet("teamview")]
        public async Task<IActionResult> TeamViewAsync()
        {
            return View();
        }

        [HttpGet("feeds")]
        public async Task<IActionResult> FeedsAsync()
        {
            return View();
        }

        [HttpPost("feeds/regenerate")]
        public async Task<IActionResult> RegenerateAsync()
        {
            return View();
        }

        [HttpGet("leave-summary/{int:int}")]
        public async Task<IActionResult> SummaryAsync(int id)
        {
            return View();
        }
    }
}