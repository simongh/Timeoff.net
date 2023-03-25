using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Departments
{
    public record GetDepartmentCommand : IRequest<EditDepartmentViewModel?>, Commands.IValidated
    {
        public int Id { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class GetDepartmentCommandHandler : IRequestHandler<GetDepartmentCommand, EditDepartmentViewModel?>
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

        public async Task<EditDepartmentViewModel?> Handle(GetDepartmentCommand request, CancellationToken cancellationToken)
        {
            var dept = await _dataContext.Departments
                .Where(d => d.DepartmentId == request.Id && d.CompanyId == _currentUserService.CompanyId)
                .Select(d => new EditDepartmentViewModel
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
                        Value = u.FirstName + " " + u.LastName,
                    })
                })
                .FirstOrDefaultAsync();

            return dept;
        }
    }
}