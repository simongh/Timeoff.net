using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record DeleteDepartmentCommand : IRequest<ResultModels.DepartmentsViewModel>
    {
        public int Id { get; init; }
    }

    internal class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, ResultModels.DepartmentsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public DeleteDepartmentCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.DepartmentsViewModel> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dept = await _dataContext.Departments
                .Where(d => d.DepartmentId == request.Id && d.CompanyId == _currentUserService.CompanyId)
                .Select(d => new
                {
                    Department = d,
                    Users = d.Users.Count,
                })
                .FirstOrDefaultAsync();

            ResultModels.FlashResult messages;
            if (dept == null)
            {
                messages = ResultModels.FlashResult.WithError("Unable to find department");
            }
            else if (dept.Users > 0)
            {
                messages = ResultModels.FlashResult.WithError($"Department '{dept.Department.Name}' cannot be removed as it still has {dept.Users} employee(s)");
            }
            else
            {
                _dataContext.Departments.Remove(dept.Department);
                await _dataContext.SaveChangesAsync();

                messages = ResultModels.FlashResult.Success($"Department '{dept.Department.Name}' was successfully removed");
            }

            var vm = await _dataContext.QueryDepartments(_currentUserService.CompanyId);
            vm.Result = messages;

            return vm;
        }
    }
}