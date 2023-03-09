using FluentValidation.Results;

namespace Timeoff.Commands
{
    public interface IValidated
    {
        IEnumerable<ValidationFailure>? Failures { get; set; }
    }
}