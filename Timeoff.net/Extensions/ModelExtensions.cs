using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Timeoff
{
    public static class ModelExtensions
    {
        public static string AsString(this LeaveStatus status)
        {
            return status switch
            {
                Timeoff.LeaveStatus.New => "Pending",
                Timeoff.LeaveStatus.Approved => "Approved",
                Timeoff.LeaveStatus.Rejected => "Rejected",
                _ => ""
            };
        }
    }
}