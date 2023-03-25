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
            var vm = await _mediator.Send(new Application.Account.GetLoginCommand());
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginPostAsync(Application.Account.LoginCommand command)
        {
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

            var vm = await _mediator.Send(new Application.Account.GetRegisterCommand());

            if (vm == null)
                return Redirect("/");
            else
                return View(vm);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPostAsync(Application.Account.RegisterCommand command)
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
        public async Task<IActionResult> ForgotPasswordPostAsync(Application.Account.ForgotPasswordComand comand)
        {
            await _mediator.Send(comand);
            return View("ForgotPassword", new Application.Account.ForgotPasswordViewModel { Success = true });
        }

        [AllowAnonymous]
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromQuery] Application.Account.GetResetPasswordCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> RestPasswordPostAsync(Application.Account.ResetPasswordCommand command)
        {
            if (string.IsNullOrEmpty(command.Token) && User.Identity?.IsAuthenticated != true)
                return await ResetPasswordAsync(new());

            var vm = await _mediator.Send(command);
            return View("ResetPassword", vm);
        }
    }
}