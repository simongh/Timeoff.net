using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Company
{
    public record LeaveTypesQuery : IRequest<IEnumerable<ResultModels.ListResult>>
    { }

    internal class LeaveTypesQueryHandler(IDataContext dataContext)
        : IRequestHandler<LeaveTypesQuery, IEnumerable<ResultModels.ListResult>>
    {
        private readonly IDataContext _dataContext = dataContext;

        public async Task<IEnumerable<ResultModels.ListResult>> Handle(LeaveTypesQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.LeaveTypes
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