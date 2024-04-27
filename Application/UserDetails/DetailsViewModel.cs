namespace Timeoff.Application.UserDetails
{
    public record DetailsViewModel : Types.UserDetailsModelBase
    {
        public Types.ScheduleModel Schedule { get; init; } = null!;

        public bool ScheduleOverride { get; init; }
    }
}