using FluentValidation;

namespace Timeoff.Application.ResetPassword
{
    internal class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(m => m.NewPassword)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(m => m.ConfirmPassword)
                .Equal(m => m.NewPassword)
                .WithMessage("The passwords must match")
                .NotEmpty();
        }
    }
}