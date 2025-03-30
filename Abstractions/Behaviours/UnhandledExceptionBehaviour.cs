﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Timeoff.Behaviours
{
    internal class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

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