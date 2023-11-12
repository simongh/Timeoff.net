using FluentValidation;

namespace Timeoff.Application.PublicHolidays
{
    internal class UpdatePublicHolidayCommandValidator : AbstractValidator<UpdatePublicHolidayCommand>
    {
        public UpdatePublicHolidayCommandValidator()
        {
            RuleFor(m => m.Add)
                .SetValidator(new PublicHolidayRequestValidator()!);

            RuleForEach(m => m.PublicHolidays)
                .NotNull();

            RuleForEach(m => m.PublicHolidays)
                .SetValidator(new PublicHolidayRequestValidator());

            RuleForEach(m => m.PublicHolidays)
                .Must((m, h) => h.Date.Year == m.Year)
                .WithMessage("Holidays can only be added for the current year");
        }
    }
}