using MediatR;

namespace Timeoff.Commands
{
    public record AllowanceByTimeQuery : IRequest<ResultModels.AllowanceByTimeViewModel>
    {
        public DateTime StartDate { get; init; } = DateTime.Today.AddDays(-DateTime.Today.Day + 1);

        public DateTime EndDate { get; init; } = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day + 1);

        public int? Team { get; init; }
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
            throw new NotImplementedException();
        }
    }
}