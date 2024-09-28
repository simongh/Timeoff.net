namespace Timeoff.Application.Users
{
    internal record ImportModel
    {
        public string Email { get; init; }

        public string LastName { get; init; }

        public string FirstName { get; init; }

        public string Department { get; init; }
    }
}