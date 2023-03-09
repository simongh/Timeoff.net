using FluentValidation;

namespace Timeoff.Validators
{
    internal class LoginCommandValidator : AbstractValidator<Commands.LoginCommand>
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