using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record UsersQuery : IRequest<ResultModels.UsersViewModel>
    {
        public int? Department { get; init; }

        public bool AsCsv { get; init; }
    }

    internal class UsersQueryHandler : IRequestHandler<UsersQuery, ResultModels.UsersViewModel>
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

        public async Task<ResultModels.UsersViewModel> Handle(UsersQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId);

            if (request.Department.HasValue)
            {
                query = query.Where(u => u.DepartmentId == request.Department.Value);
            }

            var users = await query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.UserInfoResult
                {
                    Id = u.UserId,
                    Name = u.FirstName + " " + u.LastName,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department.Name,
                    IsActive = u.IsActive,
                    IsAdmin = u.IsAdmin,
                })
                .ToArrayAsync();

            var depts = await _dataContext.Departments
                .Where(d => d.CompanyId == _currentUserService.CompanyId)
                .OrderBy(d => d.Name)
                .Select(d => new ResultModels.ListItem
                {
                    Id = d.DepartmentId,
                    Value = d.Name,
                })
                .ToArrayAsync();

            var company = await _dataContext.Companies
                .Where(c => c.CompanyId == _currentUserService.CompanyId)
                .Select(c => c.Name)
                .FirstAsync();

            return new()
            {
                CompanyName = company,
                DepartmentId = request.Department,
                Departments = depts,
                Users = users,
            };
        }
    }
}