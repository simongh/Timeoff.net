namespace Timeoff.Types
{
    public abstract record UserDetailsModelBase : UserModel
    {
        public int Team { get; init; }
        public string Email { get; init; } = null!;

        public bool IsAdmin { get; init; }

        public bool AutoApprove { get; init; }

        public DateTime? StartDate { get; init; }

        public DateTime? EndDate { get; init; }
    }
}