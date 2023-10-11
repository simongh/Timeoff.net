using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Calendar
{
    public record GetTeamViewCommand : IRequest<TeamViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;

        public int Month { get; init; } = DateTime.Today.Month;
    }

    internal class GetTeamViewCommandHandler : IRequestHandler<GetTeamViewCommand, TeamViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetTeamViewCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<TeamViewModel> Handle(GetTeamViewCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .FindById(_currentUserService.UserId)
                .AsNoTracking()
                .FirstAsync();

            return new()
            {
                Name = user.Fullname,
                CurrentDate = new DateTime(request.Year, request.Month, 1),
            };
        }
    }
}