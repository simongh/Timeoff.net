using FluentValidation;

namespace Timeoff.Application.Users
{
    internal class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty();

            RuleFor(m => m.LastName)
                .NotEmpty();

            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}