using FluentValidation;

namespace Timeoff.Validators
{
    public class PublicHolidayRequestValidator : AbstractValidator<RequestModels.PublicHolidayRequest>
    {
        public PublicHolidayRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();
        }
    }
}