﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/absences")]
    [ApiController]
    [Authorize(Policy = "token")]
    public class AbsencesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public Task<IActionResult> ListAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ProducesResponseType<ResultModels.ApiResult>(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateAsync(Application.BookAbsence.BookCommand command)
        {
            if (command == null)
                return BadRequest();

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public Task<IActionResult> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public Task<IActionResult> UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}