namespace Timeoff.ResultModels
{
    public record DepartmentResult : Types.DepartmentModel
    {
        public int Id { get; init; }

        public int EmployeeCount { get; init; }
    }
}