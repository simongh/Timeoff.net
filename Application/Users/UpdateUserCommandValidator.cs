using FluentValidation;

namespace Timeoff.Application.Users
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