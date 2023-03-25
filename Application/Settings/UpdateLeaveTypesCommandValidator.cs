using FluentValidation;

namespace Timeoff.Application.Settings
{
    internal class UpdateLeaveTypesCommandValidator : AbstractValidator<UpdateLeaveTypesCommand>
    {
        public UpdateLeaveTypesCommandValidator()
        {
            RuleFor(m => m.Add)
                .SetValidator(new Validators.LeaveTypeRequestValidator()!);

            RuleForEach(m => m.LeaveTypes)
                .NotNull()
                .SetValidator(new Validators.LeaveTypeRequestValidator());
        }
    }
}