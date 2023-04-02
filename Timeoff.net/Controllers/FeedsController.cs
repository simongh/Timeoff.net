using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    public class FeedsController : Controller
    {
        [HttpGet("{token}/ical.ics")]
        public async Task<IActionResult> IcalAsync(string token)
        {
            return View();
        }
    }
}