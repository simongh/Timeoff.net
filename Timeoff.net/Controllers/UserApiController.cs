﻿using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        [HttpGet]
        public Task<IActionResult> GetUsersAsync()
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