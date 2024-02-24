using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(new
                {
                    result.Success,
                    Token = "token",
                    Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                    result.Result?.Errors,
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("token")]
        public async Task<IActionResult> TokenAsync()
        {
            return Ok(new
            {
                Token = "token"
            });
        }

        [HttpPost("logout")]
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

            if (result.Result?.Messages?.Any() == true)
                return NoContent();
            else
                return BadRequest(result.Result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(Application.Register.RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (result!.Success)
                return NoContent();
            else
                return BadRequest(result.Result);
        }
    }
}