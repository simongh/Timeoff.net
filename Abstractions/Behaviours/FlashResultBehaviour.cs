using MediatR;

namespace Timeoff.Behaviours
{
    internal class FlashResultBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult> where TRequest : notnull
    {
        private readonly Services.IFlashMessageService _messageService;

        public FlashResultBehaviour(Services.IFlashMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            var result = await next();

            if (result is Types.IFlashResult messageResult)
            {
                _messageService.SaveMessage(messageResult.Messages);
                messageResult.Messages = _messageService.FindMessages();
            }

            return result;
        }
    }
}