using MediatR;

namespace Timeoff.Application.Departments
{
    public record DepartmentsQuery : IRequest<DepartmentsViewModel>
    {
    }

    internal class DepartmentsQueryHandler : IRequestHandler<DepartmentsQuery, DepartmentsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public DepartmentsQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<DepartmentsViewModel> Handle(DepartmentsQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.QueryDepartments(_currentUserService.CompanyId);
        }
    }
}