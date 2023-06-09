﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record DeleteLeaveTypeCommand : IRequest<SettingsViewModel>
    {
        public int Id { get; init; }
    }

    internal class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, SettingsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly Services.INewLeaveService _leaveService;

        public DeleteLeaveTypeCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService,
            Services.INewLeaveService leaveService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
            _leaveService = leaveService;
        }

        public async Task<SettingsViewModel> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult messages;

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
                messages = ResultModels.FlashResult.WithError("Cannot delete leave type whilst it is in use");
            }
            else
            {
                _dataContext.LeaveTypes.Remove(leave.leave);
                await _dataContext.SaveChangesAsync();
                _leaveService.ClearLeaveTypes();

                messages = ResultModels.FlashResult.Success("Leave type was successfully removed");
            }

            var result = await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
            result.Result = messages;

            return result;
        }
    }
}