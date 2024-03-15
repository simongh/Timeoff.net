using FluentValidation;

namespace Timeoff.Validators
{
    public class UserDetailsBaseValidator : AbstractValidator<Types.UserDetailsModelBase>
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

            RuleFor(m => m.EndDate)
                .GreaterThan(m => m.StartDate);
        }
    }
}