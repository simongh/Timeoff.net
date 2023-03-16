using FluentValidation;

namespace Timeoff.Validators
{
    internal class UpdateBankHolidayCommandValidator : AbstractValidator<Commands.UpdateBankHolidayCommand>
    {
        public UpdateBankHolidayCommandValidator()
        {
            RuleFor(m => m.BankHolidays)
                .NotEmpty();

            RuleForEach(m => m.BankHolidays)
                .NotNull();

            RuleForEach(m => m.BankHolidays)
                .SetValidator(new BankHolidayRequestValidator());

            RuleForEach(m => m.BankHolidays)
                .Must((m, h) => h.Date.Year == m.Year)
                .WithMessage("Holidays can only be added for the current year");
        }
    }
}