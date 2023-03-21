using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record UpdateDepartmentCommand : Types.DepartmentModel, IRequest<ResultModels.DepartmentsViewModel?>, IValidated
    {
        public int? Id { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class NewDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, ResultModels.DepartmentsViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public NewDepartmentCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.DepartmentsViewModel?> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult messages;

            if (request.Failures.IsValid())
            {
                Entities.Department? dept;
                if (request.Id == null)
                {
                    dept = new()
                    {
                        CompanyId = _currentUserService.CompanyId,
                    };
                    _dataContext.Departments.Add(dept);
                }
                else
                {
                    dept = await _dataContext.Departments
                        .Where(d => d.DepartmentId == request.Id.Value && d.CompanyId == _currentUserService.CompanyId)
                        .FirstOrDefaultAsync();

                    if (dept == null)
                        return null;
                }

                dept.Name = request.Name;
                dept.Allowance = request.Allowance;
                dept.IsAccrued = request.IsAccruedAllowance;
                dept.IncludeBankHolidays = request.IncludePublicHolidays;
                dept.ManagerId = request.ManagerId;

                await _dataContext.SaveChangesAsync();

                if (request.Id == null)
                    messages = ResultModels.FlashResult.Success("Departments added");
                else
                    messages = ResultModels.FlashResult.Success($"Department {dept.Name} was updated");
            }
            else
            {
                messages = request.Failures.ToFlashResult();
            }

            var result = await _dataContext.QueryDepartments(_currentUserService.CompanyId);
            result.Result = messages;
            return result;
        }
    }
}