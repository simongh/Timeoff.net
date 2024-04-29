namespace Timeoff.ResultModels
{
    public record LeaveTypeResult
    {
        public string Name { get; init; } = null!;
        public string Colour { get; init; } = null!;
    }
}