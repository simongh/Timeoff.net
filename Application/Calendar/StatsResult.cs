using Timeoff.ResultModels;

namespace Timeoff.Application.Calendar
{
    public record StatsResult
    {
        public ManagerResult Manager { get; init; } = null!;

        public ListItem Team { get; init; } = null!;
    }
}