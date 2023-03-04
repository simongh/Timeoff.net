using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("requests")]
    public class RequestController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            return View();
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectAsync()
        {
            return View();
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveAsync()
        {
            return View();
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelAsync()
        {
            return View();
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeAsync()
        {
            return View();
        }
    }
}