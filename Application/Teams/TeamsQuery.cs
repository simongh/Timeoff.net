using MediatR;

namespace Timeoff.Application.Teams
{
    public record TeamsQuery : IRequest<TeamsViewModel>
    {
    }

    internal class DepartmentsQueryHandler : IRequestHandler<TeamsQuery, TeamsViewModel>
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

        public async Task<TeamsViewModel> Handle(TeamsQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.QueryDepartments(_currentUserService.CompanyId);
        }
    }
}