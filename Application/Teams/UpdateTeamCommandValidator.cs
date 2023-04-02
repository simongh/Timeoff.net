using FluentValidation;

namespace Timeoff.Application.Teams
{
    internal class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
    {
        public UpdateTeamCommandValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();

            RuleFor(m => m.Allowance)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(50);

            RuleFor(m => m.Allowance)
                .Must(i => (i * 10) % 5 == 0)
                .WithMessage("Allowance must be in half day increments");
        }
    }
}