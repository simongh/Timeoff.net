using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record GetAbsencesCommand : IRequest<AbsencesViewModel>
    {
        public int Id { get; init; }
    }

    internal class GetAbsencesCommandHandler : IRequestHandler<GetAbsencesCommand, AbsencesViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetAbsencesCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<AbsencesViewModel> Handle(GetAbsencesCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId && u.UserId == request.Id)
                .Select(u => new
                {
                    User = u.Schedule,
                    Company = u.Company.Schedule,
                    u.FirstName,
                    u.LastName,
                    u.TeamId,
                    u.IsActivated,
                    u.EndDate,
                })
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException();
            }

            return new()
            {
                Id = request.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                IsActive = user.IsActivated && (user.EndDate == null || user.EndDate > DateTime.Today),
                Summary = await _dataContext.GetAllowanceAsync(request.Id, DateTime.Today.Year),
            };
        }
    }
}