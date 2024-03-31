using Timeoff.Application.Schedule;

namespace Timeoff.Application.UserDetails
{
    public record DetailsViewModel : Types.UserDetailsModelBase
    {
        public int Team { get => TeamId; init => TeamId = value; }

        public ScheduleModel Schedule { get; init; } = null!;

        public bool ScheduleOverride { get; init; }

        public ResultModels.FlashResult? Messages { get; set; }
    }
}