﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timeoff.Controllers
{
    public class AccountController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        //[AllowAnonymous]
        //[HttpGet("login")]
        //public async Task<IActionResult> LoginAsync()
        //{
        //    var vm = await _mediator.Send(new Application.Login.GetLoginCommand());
        //    return View(vm);
        //}

        //[AllowAnonymous]
        //[HttpPost("login")]
        //public async Task<IActionResult> LoginPostAsync(Application.Login.LoginCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    if (vm.Success)
        //        return Redirect("/");
        //    else
        //        return View("Login", vm);
        //}

        //[HttpGet("logout")]
        //public async Task<IActionResult> LogoutAsync()
        //{
        //    await HttpContext.SignOutAsync();
        //    return Redirect("/");
        //}

        //[AllowAnonymous]
        //[HttpGet("register")]
        //public async Task<IActionResult> RegisterAsync()
        //{
        //    if (User.Identity?.IsAuthenticated == true)
        //        return Redirect("/");

        //    var vm = await _mediator.Send(new Application.Register.GetRegisterCommand());

        //    if (vm == null)
        //        return Redirect("/");
        //    else
        //        return View(vm);
        //}

        //[AllowAnonymous]
        //[HttpPost("register")]
        //public async Task<IActionResult> RegisterPostAsync(Application.Register.RegisterCommand command)
        //{
        //    if (User.Identity?.IsAuthenticated == true)
        //        return Redirect("/");

        //    var vm = await _mediator.Send(command);

        //    if (vm == null)
        //        return Redirect("/");
        //    else if (vm.Success)
        //        return Redirect("/");
        //    else
        //        return View("Register", vm);
        //}

        //[AllowAnonymous]
        //[HttpGet("forgot-password")]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[AllowAnonymous]
        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPasswordPostAsync(Application.ForgotPassword.ForgotPasswordComand comand)
        //{
        //    await _mediator.Send(comand);
        //    return View("ForgotPassword", new Application.ForgotPassword.ForgotPasswordViewModel { Success = true });
        //}

        //[AllowAnonymous]
        //[HttpGet("reset-password")]
        //public async Task<IActionResult> ResetPasswordAsync([FromQuery] Application.ResetPassword.GetResetPasswordCommand command)
        //{
        //    var vm = await _mediator.Send(command);
        //    return View(vm);
        //}

        //[AllowAnonymous]
        //[HttpPost("reset-password")]
        //public async Task<IActionResult> RestPasswordPostAsync(Application.ResetPassword.ResetPasswordCommand command)
        //{
        //    if (string.IsNullOrEmpty(command.Token) && User.Identity?.IsAuthenticated != true)
        //        return await ResetPasswordAsync(new());

        //    var vm = await _mediator.Send(command);
        //    return View("ResetPassword", vm);
        //}
    }
}