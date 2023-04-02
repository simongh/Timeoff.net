namespace Timeoff.ResultModels
{
    public record TeamResult : Types.TeamModel
    {
        public int Id { get; init; }

        public int EmployeeCount { get; init; }
    }
}