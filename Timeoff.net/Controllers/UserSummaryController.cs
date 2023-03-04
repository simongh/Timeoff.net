using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("users/summary")]
    public class UserSummaryController : Controller
    {
        [HttpGet("summary/{id:int}")]
        public async Task<IActionResult> SummaryAsync(int id)
        {
            return View();
        }
    }
}