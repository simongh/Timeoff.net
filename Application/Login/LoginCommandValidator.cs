using FluentValidation;

namespace Timeoff.Application.Login
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