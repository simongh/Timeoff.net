using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            command.AuthType = CookieAuthenticationDefaults.AuthenticationScheme;

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
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/");

            var vm = await _mediator.Send(new Commands.GetRegisterCommand());

            if (vm == null)
                return Redirect("/");
            else
                return View(vm);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPostAsync(Commands.RegisterCommand command)
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect("/");

            var vm = await _mediator.Send(command);

            if (vm == null)
                return Redirect("/");
            else if (vm.Success)
                return Redirect("/");
            else
                return View("Register", vm);
        }

        [AllowAnonymous]
        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordPostAsync(Commands.ForgotPasswordComand comand)
        {
            await _mediator.Send(comand);
            return View("ForgotPassword", new ResultModels.ForgotPasswordViewModel { Success = true });
        }

        [AllowAnonymous]
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromQuery] Commands.GetResetPasswordCommand command)
        {
            command.User = User;
            var vm = await _mediator.Send(command);
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> RestPasswordPostAsync(Commands.ResetPasswordCommand command)
        {
            command.User = User;

            if (string.IsNullOrEmpty(command.Token) && command.User?.Identity?.IsAuthenticated != true)
                return await ResetPasswordAsync(new());

            var vm = await _mediator.Send(command);
            return View("ResetPassword", vm);
        }
    }
}