using CsvHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Timeoff.Application.Users
{
    public record ImportCommand : IRequest<ResultModels.ApiResult>
    {
        public required Stream File { get; init; }
    }

    internal class ImportCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService) : IRequestHandler<ImportCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<ResultModels.ApiResult> Handle(ImportCommand request, CancellationToken cancellationToken)
        {
            using var reader = new StreamReader(request.File);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<ImportMap>();

            Dictionary<string, int> teamCache = [];
            var rows = csv.GetRecordsAsync<ImportModel>(cancellationToken);

            await foreach (var item in rows)
            {
                string? error = null;
                if (string.IsNullOrEmpty(item.Email))
                    error = "Email is required";

                if (string.IsNullOrEmpty(item.FirstName))
                    error = "First name is required";

                if (string.IsNullOrEmpty(item.LastName))
                    error = "Last name is required";

                if (await _dataContext.Users.AnyAsync(u => u.Email == item.Email))
                    error = $"{item.Email} already exists";

                if (error != null)
                    return new ResultModels.ApiResult
                    {
                        Errors = [error],
                    };

                var teamId = 0;
                if (teamCache.ContainsKey(item.Team!))
                    teamId = teamCache[item.Team!];
                else
                {
                    var team = await _dataContext.Teams
                        .Where(t => t.Name == item.Team)
                        .Select(t => new
                        {
                            t.TeamId,
                        })
                        .FirstOrDefaultAsync();

                    if (team == null)
                        return new ResultModels.ApiResult
                        {
                            Errors = [$"{item.Team}  was not found"]
                        };

                    teamId = team.TeamId;
                    teamCache.Add(item.Team!, teamId);
                }

                _dataContext.Users.Add(new()
                {
                    FirstName = item.FirstName!,
                    LastName = item.LastName!,
                    Email = item.Email!,
                    Password = "changeme",
                    TeamId = teamId,
                    CompanyId = _currentUserService.CompanyId,
                });
            }

            await _dataContext.SaveChangesAsync(cancellationToken);

            return new();
        }
    }
}