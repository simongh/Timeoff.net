using FluentValidation;

namespace Timeoff.Application.UserDetails
{
    internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(
            Validators.UserDetailsBaseValidator baseValidator)
        {
            Include(baseValidator);
        }
    }
}