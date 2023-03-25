using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record UpdateLeaveTypesCommand : IRequest<SettingsViewModel>, Commands.IValidated
    {
        public RequestModels.LeaveTypeRequest[]? LeaveTypes { get; init; }

        public RequestModels.LeaveTypeRequest? Add { get; init; }

        public int First { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateLeaveTypesCommandHandler : IRequestHandler<UpdateLeaveTypesCommand, SettingsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateLeaveTypesCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<SettingsViewModel> Handle(UpdateLeaveTypesCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult? messages;
            if (request.Failures.IsValid())
            {
                messages = Insert(request);
                messages += await UpdateAsync(request);

                await _dataContext.SaveChangesAsync();
            }
            else
            {
                messages = request.Failures.ToFlashResult();
            }

            var result = await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
            result.Result = messages;

            return result;
        }

        private ResultModels.FlashResult? Insert(UpdateLeaveTypesCommand request)
        {
            if (request.Add == null)
                return null;

            _dataContext.LeaveTypes.Add(new()
            {
                Name = request.Add.Name,
                Colour = request.Add.Colour,
                AutoApprove = request.Add.AutoApprove,
                UseAllowance = request.Add.UseAllowance,
                Limit = request.Add.Limit,
                SortOrder = 1,
                CompanyId = _currentUserService.CompanyId,
            });

            return ResultModels.FlashResult.Success($"Leave type {request.Add.Name} was added successfully");
        }

        private async Task<ResultModels.FlashResult?> UpdateAsync(UpdateLeaveTypesCommand request)
        {
            if (request.LeaveTypes == null)
                return null;

            var ids = request.LeaveTypes
                .Select(i => i.Id);

            var types = await _dataContext.LeaveTypes
                .Where(t => ids.Contains(t.LeaveTypeId))
                .ToArrayAsync();

            foreach (var item in types)
            {
                var leave = request.LeaveTypes.First(t => t.Id == item.LeaveTypeId);

                item.Name = leave.Name;
                item.Colour = leave.Colour;
                item.AutoApprove = leave.AutoApprove;
                item.UseAllowance = leave.UseAllowance;
                item.Limit = leave.Limit;
                item.SortOrder = request.First == item.LeaveTypeId ? 0 : 1;
            }

            return ResultModels.FlashResult.Success("Leave type changes saved");
        }
    }
}