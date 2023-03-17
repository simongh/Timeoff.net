using MediatR;

namespace Timeoff.Commands
{
    public record AllowanceByTimeQuery : IRequest<ResultModels.AllowanceByTimeViewModel>
    {
        public DateTime StartDate { get; init; } = DateTime.Today.AddDays(-DateTime.Today.Day + 1);

        public DateTime EndDate { get; init; } = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day + 1);

        public int? Department { get; init; }
    }

    internal class AllowanceByTimeQueryHandler : IRequestHandler<AllowanceByTimeQuery, ResultModels.AllowanceByTimeViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public AllowanceByTimeQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.AllowanceByTimeViewModel> Handle(AllowanceByTimeQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.Leaves
                .Where(a => a.DateStart >= request.StartDate && a.DateEnd <= request.EndDate)
                .Where(a => a.User.CompanyId == _currentUserService.CompanyId);

            if (request.Department != null)
            {
                query = query.Where(a => a.User.DepartmentId == request.Department.Value);
            }

            var summary = query.GroupBy(a => new { a.UserId, a.LeaveTypeId }, (k, v) => v.Count());
        }
    }
}