using FluentValidation;

namespace Timeoff.Validators
{
    internal class NewBankHolidayCommandValidator : AbstractValidator<Commands.NewBankHolidayCommand>
    {
        public NewBankHolidayCommandValidator()
        {
            RuleFor(m => m.Holidays)
                .NotEmpty();

            RuleForEach(m => m.Holidays)
                .NotNull();

            RuleForEach(m => m.Holidays)
                .SetValidator(new BankHolidayRequestValidator());
        }
    }
}