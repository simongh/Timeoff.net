using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("settings")]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private readonly IMediator _mediator;

        public DepartmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("departments")]
        public async Task<IActionResult> IndexAsync()
        {
            var vm = await _mediator.Send(new Commands.DepartmentsQuery());
            return View(vm);
        }

        [HttpPost("departments")]
        public async Task<IActionResult> CreateAsync(Commands.UpdateDepartmentCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpPost("departments/delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Commands.DeleteDepartmentCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("index", vm);
        }

        [HttpGet("departments/edit/{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute] Commands.GetDepartmentCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
        }

        [HttpPost("departments/edit/{id:int}")]
        public async Task<IActionResult> UpdateAsync(Commands.UpdateDepartmentCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpGet("departments/available-supervisors/{id:int}")]
        public async Task<IActionResult> AvailableSupervisorsAsync(int id)
        {
            return View();
        }
    }
}