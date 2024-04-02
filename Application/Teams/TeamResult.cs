namespace Timeoff.Application.Teams
{
    public record TeamResult : TeamModel
    {
        public int Id { get; init; }

        public ResultModels.ListResult Manager { get; init; } = null!;

        public int EmployeeCount { get; init; }
    }
}