﻿using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.ResetPassword
{
    public record ResetPasswordCommand : IRequest<ResultModels.ApiResult>, Commands.IValidated
    {
        public string? Password { get; init; }

        public string? NewPassword { get; init; }

        public string? Token { get; set; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class ResetPasswordCommandHandler(
        IDataContext dataContext,
        Services.IUsersService usersService,
        Services.ICurrentUserService currentUserService,
        Services.IEmailTemplateService emailTemplateService)
        : IRequestHandler<ResetPasswordCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.IUsersService _usersService = usersService;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly Services.IEmailTemplateService _emailTemplateService = emailTemplateService;

        public async Task<ResultModels.ApiResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.Failures.IsValid())
            {
                return new()
                {
                    Errors = request.Failures.Select(v => v.ErrorMessage),
                };
            }

            Entities.User? user = null;
            var errors = new List<string>();

            if (!string.IsNullOrEmpty(request.Token))
            {
                user = await FromTokenAsync(request);
            }
            else if (_currentUserService.IsAuthenticated)
            {
                user = await FromIdAsync();
            }

            if (user != null)
            {
                if (_currentUserService.IsAuthenticated && !_usersService.Authenticate(user.Password, request.Password))
                    errors = ["Current password is incorrect"];
                else
                {
                    user.Password = _usersService.HashPassword(request.NewPassword!);
                    user.Token = null;
                    user.IsActivated = true;
                    _dataContext.EmailAudits.Add(_emailTemplateService.ResetPassword(user));
                    await _dataContext.SaveChangesAsync();
                }
            }
            else
            {
                if (request.Token == null)
                    errors = ["Unable to find user"];
                else
                    errors = ["Unknown reset password link, please submit request again"];
            }

            return new()
            {
                Errors = errors,
            };
        }

        private async Task<Entities.User?> FromTokenAsync(ResetPasswordCommand request)
        {
            return await _dataContext.Users
                .Where(u => u.Token == request.Token)
                .FirstOrDefaultAsync();
        }

        private async Task<Entities.User?> FromIdAsync()
        {
            return await _dataContext.Users
                .FindById(_currentUserService.UserId)
                .FirstOrDefaultAsync();
        }
    }
}