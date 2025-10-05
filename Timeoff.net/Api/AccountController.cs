using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        [ProducesResponseType<ResultModels.TokenResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ForgotPasswordAsync(Application.ForgotPassword.ForgotPasswordComand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ResultModels.ApiResult>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPasswordAsync(Application.ResetPassword.ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ResultModels.ApiResult>(StatusCodes.Status400BadRequest)]
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