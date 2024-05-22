using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "token")]
    public class CompanyController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("")]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _mediator.Send(new Application.Settings.GetSettingsCommand()));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("")]
        public async Task<IActionResult> DeleteAsync(Application.DeleteCompany.DeleteCompanyCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
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

        [AllowAnonymous]
        [HttpGet("countries")]
        public async Task<IActionResult> CountriesAsync()
        {
            return Ok((await _mediator.Send(new Application.Company.ListsCommand())).Countries);
        }

        [AllowAnonymous]
        [HttpGet("time-zones")]
        public async Task<IActionResult> TimeZonesAsync()
        {
            return Ok((await _mediator.Send(new Application.Company.ListsCommand())).TimeZones);
        }

        [HttpGet("leave-types")]
        public async Task<IActionResult> LeaveTypesAsync()
        {
            return Ok(await _mediator.Send(new Application.Company.LeaveTypesQuery()));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettingsAsync(Application.Settings.UpdateSettingsCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("schedule")]
        public async Task<IActionResult> UpdateScheduleAsync(Application.Schedule.UpdateScheduleCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("backup")]
        public async Task<IActionResult> BackupAsync()
        {
            await _mediator.Send(new Application.Company.BackupCommand());
            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("carry-over")]
        public async Task<IActionResult> CarryOverAsync()
        {
            await _mediator.Send(new Application.Company.CarryOverCommand());
            return NoContent();
        }
    }
}