using FluentValidation;

namespace Timeoff.Application.Account
{
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(m => m.Username)
                .NotEmpty();

            RuleFor(m => m.Password)
                .NotEmpty();
        }
    }
}