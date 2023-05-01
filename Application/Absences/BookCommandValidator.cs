using FluentValidation;

namespace Timeoff.Application.Absences
{
    internal class BookCommandValidator : AbstractValidator<BookCommand>
    {
        public BookCommandValidator()
        {
            RuleFor(m => m.To)
                .GreaterThanOrEqualTo(m => m.From);

            When(m => m.From == m.To, () =>
            {
                RuleFor(m => m.FromPart)
                    .NotEqual(LeavePart.Afternoon)
                    .When(m => m.ToPart == LeavePart.Morning);

                RuleFor(m => m.FromPart)
                    .NotEqual(LeavePart.All)
                    .When(m => m.ToPart != LeavePart.All);

                RuleFor(m => m.ToPart)
                    .NotEqual(LeavePart.All)
                    .When(m => m.FromPart != LeavePart.All);
            });
        }
    }
}