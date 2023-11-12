using FluentValidation;

namespace Timeoff.Application.ForgotPassword
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