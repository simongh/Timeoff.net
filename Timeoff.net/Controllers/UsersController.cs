using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("users")]
    [Authorize(Roles = "Admin")]
    public class UsersController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("import")]
        public async Task<IActionResult> ImportAsync(IFormFile file, CancellationToken cancellationToken)
        {
            await _mediator.Send(new Application.Users.ImportCommand
            {
                File = file.OpenReadStream(),
            }, cancellationToken);

            return View();
        }

        [HttpGet("import-sample")]
        public async Task<IActionResult> ImportSampleAsync()
        {
            var result = await _mediator.Send(new Application.Users.SampleCommand());

            return File(result, "text/csv", "sample.csv");
        }
    }
}