using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(Application.Login.LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("token")]
        [Authorize(Policy = "cookies")]
        public async Task<IActionResult> TokenAsync()
        {
            return Ok(await _mediator.Send(new Application.GetToken.GetTokenCommand
            {
                User = (User.Identity as ClaimsIdentity)!
            }));
        }

        [HttpPost("logout")]
        [Authorize(Policy = "cookies")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(Application.ForgotPassword.ForgotPasswordComand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(Application.ResetPassword.ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(Application.Register.RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (result!.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }
    }
}