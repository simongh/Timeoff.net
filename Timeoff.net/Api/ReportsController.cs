using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "token", Roles = Roles.Admin)]
    public class ReportsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("allowance-usage")]
        public async Task<IActionResult> AllowanceUsageAsync([FromQuery] Application.AllowanceUsage.AllowanceUsageQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}