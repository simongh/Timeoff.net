using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/calendar");
            else
                return Redirect("/login");
        }
    }
}