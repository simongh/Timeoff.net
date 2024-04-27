using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.CreateUser
{
    public record CreateCommand : Types.UserDetailsModelBase, IRequest<ResultModels.ApiResult>, Commands.IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class CreateCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService,
        Services.INewLeaveService leaveService,
        Services.IUsersService usersService,
        Services.IEmailTemplateService templateService)
        : IRequestHandler<CreateCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly Services.INewLeaveService _leaveService = leaveService;
        private readonly Services.IUsersService _usersService = usersService;
        private readonly Services.IEmailTemplateService _templateService = templateService;

        public async Task<ResultModels.ApiResult> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();
            if (request.Failures != null)
            {
                errors.AddRange(request.Failures);
            }

            var teamValid = await _dataContext.Teams
                .Where(d => d.CompanyId == _currentUserService.CompanyId && d.TeamId == request.Team)
                .AnyAsync();
            if (!teamValid)
            {
                errors.Add(new(nameof(request.Team), "Team could not be found"));
            }

            var emailUsed = await _dataContext.Users
                .Where(u => u.Email == request.Email)
                .AnyAsync();
            if (emailUsed)
            {
                errors.Add(new(nameof(request.Email), "The email is already in use"));
            }

            if (errors.Any())
            {
                return new()
                {
                    Errors = errors.Select(v => v.ErrorMessage),
                };
            }

            var user = new Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                StartDate = request.StartDate!.Value,
                EndDate = request.EndDate,
                TeamId = request.Team,
                AutoApprove = request.AutoApprove,
                IsAdmin = request.IsAdmin,
                IsActivated = true,
                CompanyId = _currentUserService.CompanyId,
                Password = "changeme",
                Token = _usersService.Token(),
            };

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            _leaveService.ClearEmployeeCache();

            var thisUser = await _dataContext.Users
                .Where(u => u.UserId == _currentUserService.UserId)
                .FirstAsync();

            _dataContext.EmailAudits.Add(_templateService.NewUser(user, thisUser));
            await _dataContext.SaveChangesAsync();

            return new();
        }
    }
}