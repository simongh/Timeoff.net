using FluentValidation;

namespace Timeoff.Application.Users
{
    internal class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator(
            UserDetailsBaseValidator baseValidator)
        {
            Include(baseValidator);
        }
    }
}