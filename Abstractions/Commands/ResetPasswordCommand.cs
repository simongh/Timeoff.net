using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Timeoff.Services;

namespace Timeoff.Commands
{
    public record ResetPasswordCommand : IRequest<ResultModels.ResetPasswordViewModel>, IValidated
    {
        public string? Password { get; init; }

        public string? NewPassword { get; init; }

        public string? ConfirmPassword { get; init; }

        public string? Token { get; set; }

        public ClaimsPrincipal? User { get; set; }

        internal bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResultModels.ResetPasswordViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.IUsersService _usersService;
        private readonly IEmailTemplateService _emailTemplateService;

        public ResetPasswordCommandHandler(
            IDataContext dataContext,
            Services.IUsersService usersService,
            Services.IEmailTemplateService emailTemplateService)
        {
            _dataContext = dataContext;
            _usersService = usersService;
            _emailTemplateService = emailTemplateService;
        }

        public async Task<ResultModels.ResetPasswordViewModel> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures != null)
            {
                return new()
                {
                    ShowCurrent = request.IsAuthenticated,
                    Token = request.Token,
                    Result = new()
                    {
                        Errors = request.Failures.Select(e => e.ErrorMessage),
                    },
                };
            }

            Entities.User? user = null;
            ResultModels.FlashResult result;

            if (!string.IsNullOrEmpty(request.Token))
            {
                user = await FromTokenAsync(request);
            }
            else if (request.IsAuthenticated)
            {
                user = await FromIdAsync(request);
            }

            if (user != null)
            {
                if (request.IsAuthenticated && !_usersService.Authenticate(user.Password, request.Password))
                    result = ResultModels.FlashResult.WithError("Current password is incorrect");
                else
                {
                    user.Password = _usersService.HashPassword(request.NewPassword!);
                    user.Token = null;
                    _dataContext.EmailAudits.Add(_emailTemplateService.ResetPassword(user));
                    await _dataContext.SaveChangesAsync();
                    result = ResultModels.FlashResult.Success("Please use new password to login into system");
                }
            }
            else
            {
                if (request.Token == null)
                    result = ResultModels.FlashResult.WithError("Unable to find user");
                else
                    result = ResultModels.FlashResult.WithError("Unknown reset password link, please submit request again");
            }

            return new()
            {
                Result = result,
                Token = request.Token,
                ShowCurrent = request.IsAuthenticated
            };
        }

        private async Task<Entities.User?> FromTokenAsync(ResetPasswordCommand request)
        {
            return await _dataContext.Users
                     .Where(u => u.Token == request.Token)
                     .FirstOrDefaultAsync();
        }

        private async Task<Entities.User?> FromIdAsync(ResetPasswordCommand request)
        {
            return await _dataContext.Users
                .FindFromPrincipal(request.User)
                .FirstOrDefaultAsync();
        }
    }
}