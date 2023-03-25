namespace Timeoff.ResultModels
{
    public record UserInfoResult
    {
        public int Id { get; init; }

        public bool IsActive { get; init; }

        public string Name { get; init; } = null!;

        public int DepartmentId { get; init; }

        public string DepartmentName { get; init; } = null!;

        public bool IsAdmin { get; init; }

        public int AvailableAllowance { get; init; }

        public int DaysUsed { get; init; }
    }
}