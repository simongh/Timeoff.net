﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("settings/bankholidays")]
    [Authorize(Roles = "Admin")]
    public class BankHolidayController : Controller
    {
        private readonly IMediator _mediator;

        public BankHolidayController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> IndexAsync([FromQuery] Commands.GetBankHolidaysQuery query)
        {
            var vm = await _mediator.Send(query);
            return View(vm);
        }

        [HttpPost()]
        public async Task<IActionResult> NewAsync(Commands.NewBankHolidayCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportAsync()
        {
            return View();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(Commands.DeleteHolidayCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("index", vm);
        }
    }
}