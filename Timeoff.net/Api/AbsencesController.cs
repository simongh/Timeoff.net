using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Api
{
    [Route("api/absences")]
    [ApiController]
    public class AbsencesController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> ListAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IActionResult> CreateAsync()
        {
            throw new NotImplementedException();
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