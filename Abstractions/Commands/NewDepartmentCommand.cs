using FluentValidation.Results;
using MediatR;

namespace Timeoff.Commands
{
    public record NewDepartmentCommand : Types.DepartmentModel, IRequest<ResultModels.DepartmentsViewModel>, IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class NewDepartmentCommandHandler : IRequestHandler<NewDepartmentCommand, ResultModels.DepartmentsViewModel>
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

        public async Task<ResultModels.DepartmentsViewModel> Handle(NewDepartmentCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult messages;

            if (request.Failures == null)
            {
                var dept = new Entities.Department
                {
                    Name = request.Name,
                    Allowance = request.Allowance,
                    IsAccrued = request.IsAccruedAllowance,
                    IncludeBankHolidays = request.IncludePublicHolidays,
                    CompanyId = _currentUserService.CompanyId,
                    ManagerId = request.ManagerId,
                };

                _dataContext.Departments.Add(dept);
                await _dataContext.SaveChangesAsync();

                messages = ResultModels.FlashResult.Success("New department added");
            }
            else
            {
                messages = new()
                {
                    Errors = request.Failures.Select(e => e.ErrorMessage),
                };
            }

            var result = await _dataContext.QueryDepartments(_currentUserService.CompanyId);
            result.Result = messages;
            return result;
        }
    }
}