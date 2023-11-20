using System.Collections.Concurrent;

namespace Timeoff.Services
{
    internal class FlashMessageService : IFlashMessageService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private static readonly ConcurrentDictionary<Guid, ResultModels.FlashResult> _messages = new();

        public FlashMessageService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public ResultModels.FlashResult? FindMessages()
        {
            if (_contextAccessor.HttpContext!.Request.Query.TryGetValue("result", out var keys))
            {
                if (Guid.TryParse(keys, out var key))
                {
                    if (_messages.TryRemove(key, out var result))
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public void SaveMessage(ResultModels.FlashResult? result)
        {
            if (result == null)
                return;

            var key = Guid.NewGuid();
            _messages.TryAdd(key, result);

            result.Result = key;
        }
    }
}