using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/public-holidays")]
    [ApiController]
    public class PublicHolidaysController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{year:int}")]
        public async Task<IActionResult> QueryAsync([FromRoute] Application.PublicHolidays.PublicHolidaysQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.PublicHolidays.DeleteHolidayCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return NoContent();
            else
                return BadRequest(result);
        }
    }
}