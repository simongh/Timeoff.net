using MediatR;

namespace Timeoff.Commands
{
    public record DepartmentsQuery : IRequest<ResultModels.DepartmentsViewModel>
    {
    }

    internal class DepartmentsQueryHandler : IRequestHandler<DepartmentsQuery, ResultModels.DepartmentsViewModel>
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

        public async Task<ResultModels.DepartmentsViewModel> Handle(DepartmentsQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.QueryDepartments(_currentUserService.CompanyId);
        }
    }
}