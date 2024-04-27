using FluentValidation;
using MediatR;
using Timeoff.Commands;

namespace Timeoff.Behaviours
{
    internal class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(e => e.Errors.Any())
                    .SelectMany(e => e.Errors)
                    .ToList();

                if (failures.Any())
                {
                    if (request is IValidated validated)
                    {
                        validated.Failures = failures;
                    }
                    else
                        throw new ValidationException();
                }
            }

            return await next();
        }
    }
}