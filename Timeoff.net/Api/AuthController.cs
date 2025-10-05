using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [Authorize(Policy = "cookies")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("token")]
        [ProducesResponseType<ResultModels.TokenResult>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> TokenAsync()
        {
            var result = await _mediator.Send(new Application.GetToken.GetTokenCommand
            {
                User = (User.Identity as ClaimsIdentity)!
            });

            if (result.Success)
                return Ok(result);
            else
                return StatusCode(StatusCodes.Status403Forbidden);
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();

            return NoContent();
        }
    }
}