using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("settings")]
    [Authorize(Roles = "Admin")]
    public class TeamsController : Controller
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet("teams")]
        //public async Task<IActionResult> IndexAsync()
        //{
        //    var vm = await _mediator.Send(new Application.Teams.TeamsQuery());
        //    return View(vm);
        //}

        [HttpPost("teams")]
        public async Task<IActionResult> CreateAsync(Application.TeamDetails.UpdateTeamCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpPost("teams/delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Application.DeleteTeam.DeleteTeamCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("index", vm);
        }

        [HttpGet("teams/edit/{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute] Application.TeamDetails.GetTeamCommand command)
        {
            var vm = await _mediator.Send(command);
            return View(vm);
        }

        [HttpPost("teams/edit/{id:int}")]
        public async Task<IActionResult> UpdateAsync(Application.TeamDetails.UpdateTeamCommand command)
        {
            var vm = await _mediator.Send(command);
            return View("Index", vm);
        }

        [HttpGet("teams/available-supervisors/{id:int}")]
        public async Task<IActionResult> AvailableSupervisorsAsync(int id)
        {
            return View();
        }
    }
}