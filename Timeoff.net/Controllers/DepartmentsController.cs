﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("settings")]
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
        public async Task<IActionResult> CreateAsync(Commands.NewDepartmentCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpPost("departments/delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return View();
        }

        [HttpGet("departments/edit/{id:int}")]
        public async Task<IActionResult> GetForUpdateAsync(int id)
        { return View(); }

        [HttpPost("departments/edit/{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id)
        { return View(); }

        [HttpGet("departments/available-supervisors/{id:int}")]
        public async Task<IActionResult> AvailableSupervisorsAsync(int id)
        {
            return View();
        }
    }
}