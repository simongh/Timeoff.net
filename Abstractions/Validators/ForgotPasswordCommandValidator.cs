using FluentValidation;

namespace Timeoff.Validators
{
    internal class ForgotPasswordCommandValidator : AbstractValidator<Commands.ForgotPasswordComand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}