using FluentValidation;

namespace Timeoff.Application.Account
{
    internal class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordComand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}