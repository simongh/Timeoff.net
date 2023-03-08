namespace Timeoff.Types
{
    public abstract record RegisterModel
    {
        public string? CompanyName { get; init; }

        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        public string? Email { get; init; }

        public string? Password { get; init; }

        public string? ConfirmPassword { get; init; }

        public string? Country { get; init; }

        public string? TimeZone { get; init; }
    }
}