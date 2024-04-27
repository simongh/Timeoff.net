using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Timeoff.Behaviours
{
    internal class LoggingBehaviour<TRequest>(ILogger<TRequest> logger)
        : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger _logger = logger;

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request: {@Request}", request);

            return Task.CompletedTask;
        }
    }
}