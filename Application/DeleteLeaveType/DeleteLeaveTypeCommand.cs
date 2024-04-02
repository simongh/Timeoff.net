using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.DeleteLeaveType
{
    public record DeleteLeaveTypeCommand : IRequest<ResultModels.ApiResult>
    {
        public int Id { get; init; }
    }

    internal class DeleteLeaveTypeCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService,
        Services.INewLeaveService leaveService)
        : IRequestHandler<DeleteLeaveTypeCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly Services.INewLeaveService _leaveService = leaveService;

        public async Task<ResultModels.ApiResult> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leave = await _dataContext.LeaveTypes
                .Where(t => t.LeaveTypeId == request.Id && t.CompanyId == _currentUserService.CompanyId)
                .Select(t => new
                {
                    leave = t,
                    inUse = t.Leaves.Any(),
                })
                .FirstOrDefaultAsync();

            if (leave == null)
            {
                throw new NotFoundException();
            }
            else if (leave.inUse)
            {
                return new()
                {
                    Errors = ["Cannot delete leave type whilst it is in use"]
                };
            }
            else
            {
                _dataContext.LeaveTypes.Remove(leave.leave);
                await _dataContext.SaveChangesAsync();
                _leaveService.ClearLeaveTypes();
            }

            return new();
        }
    }
}