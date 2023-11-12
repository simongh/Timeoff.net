using FluentValidation;

namespace Timeoff.Application.CreateUser
{
    internal class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator(
            Validators.UserDetailsBaseValidator baseValidator)
        {
            Include(baseValidator);
        }
    }
}