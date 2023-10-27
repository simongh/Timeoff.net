namespace Timeoff.Application.Calendar
{
    public record ManagerResult
    {
        public string Name { get; init; } = null!;

        public string Email { get; init; } = null!;
    }
}