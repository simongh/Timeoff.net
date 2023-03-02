using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("bankholidays")]
    public class BankHolidayController : Controller
    {
        [HttpGet()]
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> NewAsync()
        {
            return View();
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportAsync()
        {
            return View();
        }

        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return View();
        }
    }
}