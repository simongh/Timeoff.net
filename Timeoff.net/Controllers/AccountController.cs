using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync()
        {
            var vm = await _mediator.Send(new Commands.GetLoginCommand());
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginPostAsync(Commands.LoginCommand command)
        {
            command.SignInFunc = p => HttpContext.SignInAsync(p);

            var vm = await _mediator.Send(command);
            if (vm.Success)
                return Redirect("/");
            else
                return View("Login", vm);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public async Task<IActionResult> RegisterAsync()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPostAsync()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync()
        {
            return View();
        }

        [AllowAnonymous]
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