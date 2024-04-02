using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.DeleteUser
{
    public record DeleteUserCommand : IRequest<ResultModels.ApiResult>
    {
        public int Id { get; init; }
    }

    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public DeleteUserCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.ApiResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            var user = await _dataContext.Users
                .Where(u => u.UserId == request.Id && u.CompanyId == _currentUserService.CompanyId)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            var isManager = await _dataContext.Teams
                .Where(t => t.ManagerId == request.Id)
                .AnyAsync();
            if (isManager)
                errors.Add("User manages a team");
            else
            {
                var leaves = await _dataContext.Leaves
                .Where(l => l.UserId == request.Id)
                .ToArrayAsync();
                _dataContext.Leaves.RemoveRange(leaves);

                var adjustments = await _dataContext.UserAllowanceAdjustments
                    .Where(a => a.UserId == request.Id)
                    .ToArrayAsync();
                _dataContext.UserAllowanceAdjustments.RemoveRange(adjustments);

                _dataContext.Users.Remove(user);
                await _dataContext.SaveChangesAsync();
            }

            return new()
            {
                Errors = errors,
            };
        }
    }
}