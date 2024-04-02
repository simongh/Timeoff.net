namespace Timeoff.Application.Settings
{
    public record SettingsResult : SettingsModel
    {
        public Types.ScheduleModel Schedule { get; init; } = null!;
        public IEnumerable<LeaveTypeResult> LeaveTypes { get; init; } = null!;
    }
}