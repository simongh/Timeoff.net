using MediatR;

namespace Timeoff.Application.AllowanceUsage
{
    public record AllowanceUsageQuery : IRequest<AllowanceByTimeViewModel>
    {
        public DateTime StartDate { get; init; } = DateTime.Today.AddDays(-DateTime.Today.Day + 1);

        public DateTime EndDate { get; init; } = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day + 1);

        public int? Team { get; init; }
    }

    internal class AllowanceByTimeQueryHandler : IRequestHandler<AllowanceUsageQuery, AllowanceByTimeViewModel>
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

        public async Task<AllowanceByTimeViewModel> Handle(AllowanceUsageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}