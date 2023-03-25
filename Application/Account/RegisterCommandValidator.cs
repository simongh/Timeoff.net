using FluentValidation;

namespace Timeoff.Application.Account
{
    internal class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(m => m.CompanyName)
                .NotEmpty();

            RuleFor(m => m.FirstName)
                .NotEmpty();

            RuleFor(m => m.LastName)
                .NotEmpty();

            RuleFor(m => m.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(m => m.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(m => m.ConfirmPassword)
                .Equal(m => m.Password)
                .WithMessage("The passwords must match")
                .NotEmpty();

            RuleFor(m => m.Country)
                .NotEmpty()
                .Must(m => Services.CountriesService.Countries.Any(c => c.Code == m));

            RuleFor(m => m.TimeZone)
                .NotEmpty()
                .Must(m => Services.TimeZoneService.TimeZones.Any(tz => tz.Id == m));
        }
    }
}