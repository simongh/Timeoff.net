﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] Application.Users.UsersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Application.CreateUser.CreateCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Messages?.Errors?.Any() == true)
                return BadRequest(result.Messages);
            else
                return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] Application.UserDetails.GetDetailsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, Application.UserDetails.UpdateUserCommand command)
        {
            if (command == null)
                return BadRequest();

            command.Id = id;

            var result = await _mediator.Send(command);

            if (result.Messages?.Errors?.Any() == true)
                return BadRequest(result.Messages);
            else
                return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeleteUser.DeleteUserCommand command)
        {
            if (command == null)
                return BadRequest();

            var result = await _mediator.Send(command);

            if (result.Messages?.Errors?.Any() == true)
                return BadRequest(result.Messages);
            else
                return NoContent();
        }

        [HttpPost("{id:int}/reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromRoute] Application.ResetPassword.ResetUserPasswordCommand command)
        {
            await _mediator.Send(command);

            return Accepted();
        }

        [HttpPut("{id:int}/schedule")]
        public async Task<IActionResult> UpdateScheduleAsync(int id, Application.Schedule.ScheduleModel? schedule)
        {
            var result = await _mediator.Send(new Application.Schedule.UpdateUserScheduleCommand
            {
                Id = id,
                Schedule = schedule
            });

            return Ok(result);
        }

        [HttpPut("{id:int}/adjustments")]
        public async Task<IActionResult> UpdateAdjustmentsAsync(int id, Application.Absences.UpdateAbsencesCommand command)
        {
            if (command == null)
                return BadRequest();

            command.Id = id;

            var result = await _mediator.Send(command);

            if (result.Messages?.Errors?.Any() == true)
                return BadRequest(result.Messages);
            else
                return Ok(result.Summary);
        }
    }
}