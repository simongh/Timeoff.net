using FluentValidation;

namespace Timeoff.Validators
{
    internal class UpdateLeaveTypesCommandValidator : AbstractValidator<Commands.UpdateLeaveTypesCommand>
    {
        public UpdateLeaveTypesCommandValidator()
        {
            RuleFor(m => m.Add)
                .SetValidator(new LeaveTypeRequestValidator()!);

            RuleForEach(m => m.LeaveTypes)
                .NotNull()
                .SetValidator(new LeaveTypeRequestValidator());
        }
    }
}