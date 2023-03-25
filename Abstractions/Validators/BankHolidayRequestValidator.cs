using FluentValidation;

namespace Timeoff.Validators
{
    internal class BankHolidayRequestValidator : AbstractValidator<RequestModels.PublicHolidayRequest>
    {
        public BankHolidayRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();
        }
    }
}