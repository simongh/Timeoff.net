using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginPostAsync()
        {
            return View();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet("register")]
        public async Task<IActionResult> RegisterAsync()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterPostAsync()
        {
            return View();
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync()
        {
            return View();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordPostAsync()
        {
            return View();
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync()
        {
            return View();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> RestPasswordPostAsync()
        {
            return View();
        }
    }
}