namespace Timeoff
{
    public enum WorkingDay : byte
    {
        WholeDay,
        None,
        Morning,
        Afternoon,
    }

    public enum LeaveStatus : byte
    {
        New,
        Approved,
        Rejected,
        PendingRevoke,
        Cancelled
    }

    public enum LeavePart : byte
    {
        All,
        Morning,
        Afternoon,
    }

    public enum FeedType : byte
    {
        Calendar,
        Wallchart,
        TeamView,
        Company
    }
}