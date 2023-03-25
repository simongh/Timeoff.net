using FluentValidation;

namespace Timeoff.Application.Settings
{
    internal class UpdateSettingsCommandValidator : AbstractValidator<UpdateSettingsCommand>
    {
        public UpdateSettingsCommandValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty();

            RuleFor(m => m.CarryOver)
                .GreaterThanOrEqualTo(0);

            RuleFor(m => m.CarryOver)
                .Must(i => i <= 21 || i == 1000);

            RuleFor(m => m.TimeZone)
                .Must(tz => Services.TimeZoneService.TimeZones.Any(c => c.Id == tz));

            RuleFor(m => m.Country)
                .Must(cc => Services.CountriesService.Countries.Any(c => c.Code == cc));
        }
    }
}