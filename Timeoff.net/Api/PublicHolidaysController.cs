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
        public Task<IActionResult> CreateAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> UpdateAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public Task<IActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}