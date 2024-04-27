using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Company
{
    public record LeaveTypesQuery : IRequest<IEnumerable<ResultModels.ListResult>>
    { }

    internal class LeaveTypesQueryHandler(
        Services.ICurrentUserService currentUserService,
        IDataContext dataContext)
        : IRequestHandler<LeaveTypesQuery, IEnumerable<ResultModels.ListResult>>
    {
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<ResultModels.ListResult>> Handle(LeaveTypesQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.LeaveTypes.Where(lt => lt.CompanyId == _currentUserService.CompanyId)
                .OrderBy(lt => lt.Name)
                .Select(lt => new ResultModels.ListResult
                {
                    Id = lt.LeaveTypeId,
                    Name = lt.Name,
                })
                .ToArrayAsync();
        }
    }
}