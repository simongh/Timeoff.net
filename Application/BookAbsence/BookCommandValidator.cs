using FluentValidation;

namespace Timeoff.Application.BookAbsence
{
    internal class BookCommandValidator : AbstractValidator<BookCommand>
    {
        public BookCommandValidator()
        {
            RuleFor(m => m.End)
                .GreaterThanOrEqualTo(m => m.Start);

            When(m => m.Start == m.End, () =>
            {
                RuleFor(m => m.StartPart)
                    .NotEqual(LeavePart.Afternoon)
                    .When(m => m.EndPart == LeavePart.Morning);

                RuleFor(m => m.StartPart)
                    .NotEqual(LeavePart.All)
                    .When(m => m.EndPart != LeavePart.All);

                RuleFor(m => m.EndPart)
                    .NotEqual(LeavePart.All)
                    .When(m => m.StartPart != LeavePart.All);
            });
        }
    }
}