using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.ForgotPassword
{
    public record ForgotPasswordComand : IRequest, Commands.IValidated
    {
        public string Email { get; init; } = null!;
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class ForgotPasswordCommandHandler(
        IDataContext dataContext,
        Services.IEmailTemplateService emailTemplateService,
        Services.IUsersService usersService)
        : IRequestHandler<ForgotPasswordComand>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.IEmailTemplateService _emailTemplateService = emailTemplateService;
        private readonly Services.IUsersService _usersService = usersService;

        public async Task Handle(ForgotPasswordComand request, CancellationToken cancellationToken)
        {
            if (!request.Failures.IsValid())
                return;

            var user = await _dataContext.Users
                .FindByEmail(request.Email)
                .Where(u => u.IsActivated)
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