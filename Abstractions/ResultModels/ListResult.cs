namespace Timeoff.ResultModels
{
    public record ListResult
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;
    }
}