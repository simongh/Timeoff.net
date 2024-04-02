using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record UpdateLeaveTypesCommand : IRequest<ResultModels.ApiResult<IEnumerable<LeaveTypeResult>>>, Commands.IValidated
    {
        public LeaveTypeRequest[]? LeaveTypes { get; init; }

        public LeaveTypeRequest? Add { get; init; }

        public int First { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateLeaveTypesCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UpdateLeaveTypesCommand, ResultModels.ApiResult<IEnumerable<LeaveTypeResult>>>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<ResultModels.ApiResult<IEnumerable<LeaveTypeResult>>> Handle(UpdateLeaveTypesCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures.IsValid())
            {
                Insert(request);
                await UpdateAsync(request);

                await _dataContext.SaveChangesAsync();
            }
            else
            {
                return new()
                {
                    Errors = request.Failures.Select(v => v.ErrorMessage)
                };
            }

            return new()
            {
                Result = await _dataContext.LeaveTypes
                    .Where(lt => lt.CompanyId == _currentUserService.CompanyId)
                    .ToModels()
                    .AsQueryable()
                    .ToArrayAsync(),
            };
        }

        private void Insert(UpdateLeaveTypesCommand request)
        {
            if (request.Add == null)
                return;

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
        }

        private async Task UpdateAsync(UpdateLeaveTypesCommand request)
        {
            if (request.LeaveTypes == null)
                return;

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
        }
    }
}