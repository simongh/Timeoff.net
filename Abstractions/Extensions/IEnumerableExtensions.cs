using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Timeoff
{
    internal static class IEnumerableExtensions
    {
        public static ResultModels.FlashResult ToFlashResult(this IEnumerable<ValidationFailure> failures)
        {
            return new()
            {
                Errors = failures.Select(e => e.ErrorMessage),
            };
        }

        public static bool IsValid([NotNullWhen(false)] this IEnumerable<ValidationFailure>? failures)
        {
            return failures?.Any() != true;
        }
    }
}