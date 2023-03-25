using MediatR;
using Microsoft.EntityFrameworkCore;
using Timeoff.Services;

namespace Timeoff.Commands
{
    public record EmailAuditQuery : IRequest<ResultModels.EmailAuditViewModel>
    {
        public DateTime? Start { get; init; }

        public DateTime? End { get; init; }

        public int? UserId { get; init; }

        public int Page { get; init; } = 1;
    }

    internal class EmailAuditQueryHandler : IRequestHandler<EmailAuditQuery, ResultModels.EmailAuditViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly ICurrentUserService _currentUserService;

        public EmailAuditQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.EmailAuditViewModel> Handle(EmailAuditQuery request, CancellationToken cancellationToken)
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

            var pager = new ResultModels.PagerResult
            {
                CurrentPage = request.Page,
                TotalRows = await query.CountAsync(),
                QueryParameters = new
                {
                    request.UserId,
                    request.Start,
                    request.End,
                },
            };

            var emails = await query
                .OrderByDescending(e => e.CreatedAt)
                .Skip((pager.CurrentPage - 1) * pager.PageSize)
                .Take(pager.PageSize)
                .Select(e => new ResultModels.EmailResult
                {
                    Id = e.EmailAuditId,
                    Subject = e.Subject,
                    Body = e.Body,
                    Name = e.User.FirstName + " " + e.User.LastName,
                    UserId = e.UserId,
                    CreatedAt = e.CreatedAt,
                    Email = e.User.Email,
                })
                .ToArrayAsync();

            var users = await _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId)
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new ResultModels.ListItem
                {
                    Id = u.UserId,
                    Value = u.FirstName + " " + u.LastName,
                })
                .ToArrayAsync();

            var dateFormat = await _dataContext.Companies
                .Where(c => c.CompanyId == _currentUserService.CompanyId)
                .Select(c => c.DateFormat)
                .FirstAsync();

            return new ResultModels.EmailAuditViewModel
            {
                Start = request.Start,
                End = request.End,
                UserId = request.UserId,
                Emails = emails,
                Users = users,
                Pager = pager,
                DateFormat = dateFormat,
            };
        }
    }
}