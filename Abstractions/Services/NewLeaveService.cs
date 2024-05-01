using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Timeoff.ResultModels;

namespace Timeoff.Services
{
    internal class NewLeaveService : INewLeaveService
    {
        private readonly IDataContext _dataContext;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUserService;

        public int MyId => _currentUserService.UserId;

        public string DateFormat => _currentUserService.DateFormat;

        private string LeaveTypeKey => $"leavetypes_{_currentUserService.CompanyId}";
        private string UsersKey => $"users_{_currentUserService.CompanyId}";

        public NewLeaveService(
            IDataContext dataContext,
            IMemoryCache cache,
            ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _cache = cache;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ListItem>> EmployeesIManageAsync()
        {
            var employees = await _cache.GetOrCreateAsync(UsersKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);

                return await _dataContext.Users
                    .Select(u => new ManagedEmployee
                    {
                        ManagerId = u.Team.ManagerId!.Value,
                        Value = new()
                        {
                            Id = u.UserId,
                            Value = u.FirstName + " " + u.LastName,
                        },
                    })
                    .ToArrayAsync();
            });

            return employees
                .Where(e => e.ManagerId == _currentUserService.UserId)
                .Select(e => e.Value)
                .OrderBy(e => e.Value)
                .ToArray();
        }

        public void ClearEmployeeCache()
        {
            _cache.Remove(UsersKey);
        }

        public async Task<IEnumerable<ListItem>> LeaveTypesAsync()
        {
            var types = await _cache.GetOrCreateAsync(LeaveTypeKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(60);

                return await _dataContext.LeaveTypes
                    .OrderBy(t => t.Name)
                    .Select(t => new ListItem
                    {
                        Id = t.LeaveTypeId,
                        Value = t.Name,
                    })
                    .ToArrayAsync();
            });

            return types!;
        }

        public void ClearLeaveTypes()
        {
            _cache.Remove(LeaveTypeKey);
        }

        private record ManagedEmployee
        {
            public int ManagerId { get; init; }

            public ListItem Value { get; init; }
        }
    }
}