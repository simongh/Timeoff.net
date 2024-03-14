using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.CreateUser
{
    public record CreateCommand : Types.UserDetailsModelBase, IRequest<CreateViewModel>, Commands.IValidated
    {
        public int Team
        {
            get => TeamId;
            init => TeamId = value;
        }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class CreateCommandHandler : IRequestHandler<CreateCommand, CreateViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly Services.INewLeaveService _leaveService;
        private readonly Services.IUsersService _usersService;
        private readonly Services.IEmailTemplateService _templateService;

        public CreateCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService,
            Services.INewLeaveService leaveService,
            Services.IUsersService usersService,
            Services.IEmailTemplateService templateService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
            _leaveService = leaveService;
            _usersService = usersService;
            _templateService = templateService;
        }

        public async Task<CreateViewModel> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
            var errors = new List<ValidationFailure>();
            if (request.Failures != null)
            {
                errors.AddRange(request.Failures);
            }

            var teamValid = await _dataContext.Teams
                .Where(d => d.CompanyId == _currentUserService.CompanyId && d.TeamId == request.TeamId)
                .AnyAsync();
            if (!teamValid)
            {
                errors.Add(new(nameof(request.TeamId), "Team could not be found"));
            }

            var emailUsed = await _dataContext.Users
                .Where(u => u.Email == request.Email)
                .AnyAsync();
            if (emailUsed)
            {
                errors.Add(new(nameof(request.Email), "The email is already in use"));
            }

            ResultModels.FlashResult messages;
            if (errors.Any())
            {
                messages = errors.ToFlashResult();

                //var vm = await _dataContext.GetCreateViewModelAsync(_currentUserService.CompanyId);

                return new()
                {
                    //FirstName = request.FirstName,
                    //LastName = request.LastName,
                    //Email = request.Email,
                    //TeamId = request.TeamId,
                    //StartDate = request.StartDate,
                    //EndDate = request.EndDate,
                    //AutoApprove = request.AutoApprove,
                    //IsAdmin = request.IsAdmin,
                    Messages = messages,
                };
            }

            var user = new Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                StartDate = request.StartDate!.Value,
                EndDate = request.EndDate,
                TeamId = request.TeamId,
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

            return new()
            {
                Messages = ResultModels.FlashResult.Success("New user account successfully added"),
            };
        }
    }
}