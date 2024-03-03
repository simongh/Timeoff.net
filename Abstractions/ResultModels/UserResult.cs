namespace Timeoff.ResultModels
{
    public record UserResult
    {
        public int Id { get; init; }

        public string Name { get; init; } = null!;
    }
}