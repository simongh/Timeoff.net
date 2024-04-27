using MediatR;
using Microsoft.Extensions.Logging;

namespace Timeoff.Behaviours
{
    internal class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Request: unhandled exception for request {name} {@request}", requestName, request);
                throw;
            }
        }
    }
}