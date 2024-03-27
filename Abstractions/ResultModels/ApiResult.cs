namespace Timeoff.ResultModels
{
    public record ApiResult
    {
        public IEnumerable<string> Errors { get; set; } = [];

        public bool IsSuccess => !Errors.Any();
    }

    public record ApiResult<T> : ApiResult
    {
        public T? Result { get; set; }
    }
}