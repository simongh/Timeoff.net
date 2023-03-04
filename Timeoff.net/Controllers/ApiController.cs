using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet("notifications")]
        public async Task<IActionResult> NotificationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}