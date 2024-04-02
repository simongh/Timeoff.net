namespace Timeoff.Application.Settings
{
    internal static class Extensions
    {
        public static IEnumerable<LeaveTypeResult> ToModels(this IEnumerable<Entities.LeaveType> leaveTypes)
        {
            return leaveTypes
                .OrderBy(t => t.Name)
                .Select(l => new LeaveTypeResult
                {
                    Name = l.Name,
                    First = l.SortOrder == 0,
                    AutoApprove = l.AutoApprove,
                    UseAllowance = l.UseAllowance,
                    Colour = l.Colour,
                    Id = l.LeaveTypeId,
                    Limit = l.Limit,
                });
        }
    }
}