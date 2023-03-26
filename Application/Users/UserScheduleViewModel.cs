namespace Timeoff.Application.Users
{
    public record UserScheduleViewModel : Types.UserModel
    {
        public IEnumerable<bool> Schedule { get; init; } = null!;

        public bool UserSpecific { get; init; }
    }
}