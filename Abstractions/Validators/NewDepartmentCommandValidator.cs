using FluentValidation;

namespace Timeoff.Validators
{
    internal class NewDepartmentCommandValidator : AbstractValidator<Commands.NewDepartmentCommand>
    {
        public NewDepartmentCommandValidator(
            DepartmentModelValidator deptValidator)
        {
            Include(deptValidator);
        }
    }
}