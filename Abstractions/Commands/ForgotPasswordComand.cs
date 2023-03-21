﻿using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record ForgotPasswordComand : IRequest, IValidated
    {
        public string Email { get; init; } = null!;
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordComand>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.IEmailTemplateService _emailTemplateService;
        private readonly Services.IUsersService _usersService;

        public ForgotPasswordCommandHandler(
            IDataContext dataContext,
            Services.IEmailTemplateService emailTemplateService,
            Services.IUsersService usersService)
        {
            _dataContext = dataContext;
            _emailTemplateService = emailTemplateService;
            _usersService = usersService;
        }

        public async Task Handle(ForgotPasswordComand request, CancellationToken cancellationToken)
        {
            if (!request.Failures.IsValid())
                return;

            var user = await _dataContext.Users
                .FindByEmail(request.Email)
                .Include(u => u.Company)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                user.Token = _usersService.Token();
                var email = _emailTemplateService.ForgotPassword(user);
                _dataContext.EmailAudits.Add(email);
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}