using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.EmailAudit
{
    public record EmailAuditQuery : IRequest<QueryResult>
    {
        public DateTime? Start { get; init; }

        public DateTime? End { get; init; }

        public int? UserId { get; init; }

        public int Page { get; init; } = 1;
    }

    internal class EmailAuditQueryHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<EmailAuditQuery, QueryResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<QueryResult> Handle(EmailAuditQuery request, CancellationToken cancellationToken)
        {
            var query = _dataContext.EmailAudits
                .Where(e => e.CompanyId == _currentUserService.CompanyId);

            if (request.Start.HasValue)
            {
                query = query.Where(e => e.CreatedAt >= request.Start.Value);
            }

            if (request.End.HasValue)
            {
                query = query.Where(e => e.CreatedAt <= request.End.Value);
            }

            if (request.UserId.HasValue)
            {
                query = query.Where(e => e.UserId == request.UserId.Value);
            }

            var pager = new PagerResult
            {
                CurrentPage = request.Page,
                TotalRows = await query.CountAsync(),
            };

            var emails = await query
                .AsNoTracking()
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(e => new EmailResult
                {
                    Id = e.EmailAuditId,
                    Subject = e.Subject,
                    Content = e.Body,
                    User = new()
                    {
                        Id = e.UserId,
                        Name = e.User.FirstName + " " + e.User.LastName,
                    },
                    CreatedAt = e.CreatedAt,
                    Email = e.User.Email,
                })
                .ToArrayAsync();

            return new()
            {
                Results = emails,
                Pages = pager.TotalPages,
            };
        }
    }
}