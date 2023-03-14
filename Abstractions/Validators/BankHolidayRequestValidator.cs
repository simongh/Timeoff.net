using FluentValidation;

namespace Timeoff.Validators
{
    internal class BankHolidayRequestValidator : AbstractValidator<RequestModels.BankHolidayRequest>
    {
        public BankHolidayRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();
        }
    }
}