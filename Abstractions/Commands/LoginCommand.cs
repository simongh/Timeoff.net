using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Timeoff.Commands
{
    public record LoginCommand : IRequest<ResultModels.LoginViewModel>, IValidated
    {
        public string? Username { get; init; }

        public string? Password { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, ResultModels.LoginViewModel>
    {
        private readonly Types.Options _options;
        private readonly Services.IUsersService _usersService;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly IDataContext _dataContext;

        public LoginCommandHandler(
            IOptions<Types.Options> options,
            Services.IUsersService usersService,
            Services.ICurrentUserService currentUserService,
            IDataContext dataContext)
        {
            _options = options.Value;
            _usersService = usersService;
            _currentUserService = currentUserService;
            _dataContext = dataContext;
        }

        public async Task<ResultModels.LoginViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var success = false;

            if (request.Failures == null)
            {
                var user = await _dataContext.Users
                .FindByEmail(request.Username)
                .FirstOrDefaultAsync(cancellationToken);

                if (user != null && _usersService.Authenticate(user.Password, request.Password))
                {
                    if (_usersService.ShouldUpgrade(user.Password))
                    {
                        user.Password = _usersService.HashPassword(request.Password!);
                    }
                    user.Token = null;
                    await _dataContext.SaveChangesAsync();

                    var userId = new ClaimsIdentity(new Claim[]
                    {
                        new ("userid",user.UserId.ToString()),
                        new ("companyid",user.CompanyId.ToString()),
                        new (ClaimTypes.Role, user.Admin ? "Admin" : "User")
                    }, _currentUserService.AuthenticationScheme);

                    await _currentUserService.SignInAsync(new(userId));
                    success = true;
                }
            }

            return new()
            {
                AllowRegistrations = _options.AllowNewAccountCreation,
                Success = success,
            };
        }
    }
}