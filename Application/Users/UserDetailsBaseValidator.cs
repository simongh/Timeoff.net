using FluentValidation;

namespace Timeoff.Application.Users
{
    internal class UserDetailsBaseValidator : AbstractValidator<UserDetailsModelBase>
    {
        public UserDetailsBaseValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty();

            RuleFor(m => m.LastName)
                .NotEmpty();

            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(m => m.StartDate)
                .NotEmpty();
        }
    }
}