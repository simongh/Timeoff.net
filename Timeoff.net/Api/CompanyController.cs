using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("")]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _mediator.Send(new Application.Settings.GetSettingsCommand()));
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAsync(Application.DeleteCompany.DeleteCompanyCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.Result?.Errors?.Any() == true)
                return BadRequest(result.Result);
            else
                return NoContent();
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeamsAsync()
        {
            return Ok(await _mediator.Send(new Application.Teams.TeamsListQuery()));
        }

        [HttpGet("users")]
        public async Task<IActionResult> UsersAsync()
        {
            return Ok(await _mediator.Send(new Application.Users.UsersListQuery()));
        }

        [HttpGet("countries")]
        public async Task<IActionResult> CountriesAsync()
        {
            return Ok((await _mediator.Send(new Application.Settings.ListsCommand())).Countries);
        }

        [HttpGet("time-zones")]
        public async Task<IActionResult> TimeZonesAsync()
        {
            return Ok((await _mediator.Send(new Application.Settings.ListsCommand())).TimeZones);
        }

        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettingsAsync(Application.Settings.UpdateSettingsCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.Result?.Errors?.Any() == true)
                return BadRequest(result.Result);
            else
                return NoContent();
        }

        [HttpPut("schedule")]
        public async Task<IActionResult> UpdateScheduleAsync(Application.Schedule.UpdateScheduleCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.Result?.Errors?.Any() == true)
                return BadRequest(result.Result);
            else
                return NoContent();
        }
    }
}