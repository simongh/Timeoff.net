namespace Timeoff.Application.Users
{
    public abstract record UserDetailsModelBase : Types.UserModel
    {
        public string Email { get; init; } = null!;

        public int DepartmentId { get; init; }

        public bool IsAdmin { get; init; }

        public bool AutoApprove { get; init; }

        public DateTime StartDate { get; init; }

        public DateTime? EndDate { get; init; }
    }
}