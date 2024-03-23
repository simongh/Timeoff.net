using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/company/leave-types")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}