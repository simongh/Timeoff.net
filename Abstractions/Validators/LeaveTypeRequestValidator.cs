using FluentValidation;

namespace Timeoff.Validators
{
    public class LeaveTypeRequestValidator : AbstractValidator<RequestModels.LeaveTypeRequest>
    {
        public LeaveTypeRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();

            RuleFor(m => m.Limit)
                .GreaterThanOrEqualTo(0);

            RuleFor(m => m.Colour)
                .Matches(@"^leave_type_color_\d$")
                .WithMessage("'{PropertyName}' must be a valid css class");
        }
    }
}