using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("integration")]
        public async Task<IActionResult> GetIntegrationAsync()
        {
            return Ok(await _mediator.Send(new Application.IntegrationApi.GetIntegrationApiCommand()));
        }

        [HttpPut("integration")]
        public async Task<IActionResult> UpdateIntegrationAsync(Application.IntegrationApi.UpdateIntegrationApiCommand command)
        {
            if (command == null)
            {
                return BadRequest();
            }

            return Ok(await _mediator.Send(command));
        }
    }
}