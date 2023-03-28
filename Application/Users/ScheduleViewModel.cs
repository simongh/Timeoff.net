namespace Timeoff.Application.Users
{
    public record ScheduleViewModel : Types.UserModel
    {
        public IEnumerable<bool> Schedule { get; init; } = null!;

        public bool UserSpecific { get; init; }

        public ResultModels.FlashResult? Messages { get; set; }
    }
}