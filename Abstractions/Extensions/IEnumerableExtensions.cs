using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Timeoff
{
    public static class IEnumerableExtensions
    {
        public static bool IsValid([NotNullWhen(false)] this IEnumerable<ValidationFailure>? failures)
        {
            return failures?.Any() != true;
        }
    }
}