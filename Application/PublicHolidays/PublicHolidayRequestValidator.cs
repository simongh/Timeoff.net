using FluentValidation;

namespace Timeoff.Application.PublicHolidays
{
    public class PublicHolidayRequestValidator : AbstractValidator<PublicHolidayRequest>
    {
        public PublicHolidayRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();
        }
    }
}