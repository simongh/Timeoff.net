using Timeoff.ResultModels;

namespace Timeoff.Services
{
    public interface INewLeaveService
    {
        int MyId { get; }

        string DateFormat { get; }

        void ClearEmployeeCache();

        void ClearLeaveTypes();

        Task<IEnumerable<ListItem>> EmployeesIManageAsync();

        Task<IEnumerable<ListItem>> LeaveTypesAsync();
    }
}