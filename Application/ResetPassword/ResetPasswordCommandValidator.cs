using FluentValidation;

namespace Timeoff.Application.ResetPassword
{
    internal class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator(Services.ICurrentUserService userService)
        {
            RuleFor(m => m.NewPassword)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(m => m.ConfirmPassword)
                .Equal(m => m.NewPassword)
                .WithMessage("The passwords must match")
                .NotEmpty();

            When(_ => userService.IsAuthenticated, () =>
            {
                RuleFor(m => m.Password)
                    .NotEmpty();

                RuleFor(m => m.Token)
                    .Null();
            });

            When(_ => !userService.IsAuthenticated, () =>
            {
                RuleFor(m => m.Password)
                    .Null();

                RuleFor(m => m.Token)
                    .NotEmpty();
            });
        }
    }
}