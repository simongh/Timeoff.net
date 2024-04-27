namespace Timeoff.Types
{
    public abstract record UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; init; } = null!;

        public string LastName { get; init; } = null!;

        public bool IsActive { get; init; }
    }
}