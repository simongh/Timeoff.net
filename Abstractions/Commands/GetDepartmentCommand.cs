using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record GetDepartmentCommand : IRequest<ResultModels.EditDepartmentViewModel?>, IValidated
    {
        public int Id { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class GetDepartmentCommandHandler : IRequestHandler<GetDepartmentCommand, ResultModels.EditDepartmentViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetDepartmentCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.EditDepartmentViewModel?> Handle(GetDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dept = await _dataContext.Departments
                .Where(d => d.DepartmentId == request.Id && d.CompanyId == _currentUserService.CompanyId)
                .Select(d => new ResultModels.EditDepartmentViewModel
                {
                    Id = d.DepartmentId,
                    Name = d.Name,
                    Allowance = d.Allowance,
                    IncludePublicHolidays = d.IncludeBankHolidays,
                    IsAccruedAllowance = d.IsAccrued,
                    ManagerId = d.ManagerId!.Value,
                    Users = d.Company.Users.Select(u => new ResultModels.ListItem
                    {
                        Id = u.UserId,
                        Value = u.Name + " " + u.LastName,
                    })
                })
                .FirstOrDefaultAsync();

            return dept;
        }
    }
}