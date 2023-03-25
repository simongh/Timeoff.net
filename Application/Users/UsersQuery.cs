using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record UsersQuery : IRequest<UsersViewModel>
    {
        public int? Department { get; init; }

        public bool AsCsv { get; init; }
    }

    internal class UsersQueryHandler : IRequestHandler<UsersQuery, UsersViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UsersQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<UsersViewModel> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId);

            if (request.Department.HasValue)
            {
                query = query.Where(u => u.DepartmentId == request.Department.Value);
            }
            var year = DateTime.Today.Year;

            var users = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.UserInfoResult
                {
                    Id = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department.Name,
                    IsActive = u.IsActive,
                    IsAdmin = u.IsAdmin,
                    AllowanceCalculator = new()
                    {
                        DaysUsed = u.Leave
                            .Where(a => a.DateStart.Year == year && a.DateEnd.Year == year)
                            .Where(a => a.Status == LeaveStatus.Approved)
                            .Sum(a => a.Days),
                        Start = u.StartDate,
                        End = u.EndDate,
                        Acrrue = u.Department.IsAccrued,
                        Adjustment = u.Adjustments
                            .Where(a => a.Year == year)
                            .FirstOrDefault()!,
                        Allowance = u.Department.Allowance,
                    }
                })
                .ToArrayAsync();

            var company = await _dataContext.Companies
                .Where(c => c.CompanyId == _currentUserService.CompanyId)
                .Select(c => new
                {
                    c.Name,
                    Departments = c.Departments
                        .OrderBy(d => d.Name)
                        .Select(d => new ResultModels.ListItem
                        {
                            Id = d.DepartmentId,
                            Value = d.Name,
                        })
                })
                .FirstAsync();

            return new()
            {
                CompanyName = company.Name,
                DepartmentId = request.Department,
                Departments = company.Departments,
                Users = users,
            };
        }
    }
}