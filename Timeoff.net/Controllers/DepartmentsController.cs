using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("settings")]
    public class DepartmentsController : Controller
    {
        [HttpGet("departments")]
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        [HttpPost("departments")]
        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }

        [HttpPost("departments/delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return View();
        }

        [HttpGet("departments/edit/{id:int}")]
        public async Task<IActionResult> GetForUpdateAsync(int id)
        { return View(); }

        [HttpPost("departments/edit/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id)
        { return View(); }

        [HttpGet("departments/available-supervisors/{id:int}")]
        public async Task<IActionResult> AvailableSupervisorsAsync(int id)
        {
            return View();
        }
    }
}