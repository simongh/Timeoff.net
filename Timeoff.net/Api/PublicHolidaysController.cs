using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/public-holidays")]
    [ApiController]
    [Authorize(Policy = "token")]
    public class PublicHolidaysController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{year:int}")]
        public async Task<IActionResult> QueryAsync([FromRoute] Application.PublicHolidays.PublicHolidaysQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Application.PublicHolidays.UpdatePublicHolidayCommand model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(model);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Application.PublicHolidays.UpdatePublicHolidayCommand model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await _mediator.Send(model);

            if (result.IsSuccess)
            {
                return NoContent();
            }
            else
                return BadRequest(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeletePublicHoliday.DeleteHolidayCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }
    }
}