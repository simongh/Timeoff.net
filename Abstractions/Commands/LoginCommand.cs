using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Timeoff.Commands
{
    public record LoginCommand : IRequest<ResultModels.LoginViewModel>
    {
        public string? Username { get; init; }

        public string? Password { get; init; }

        public string AuthType { get; set; } = null!;

        public Func<ClaimsPrincipal, Task> SignInFunc { get; set; } = null!;
    }

    internal class LoginCommandHandler : IRequestHandler<LoginCommand, ResultModels.LoginViewModel>
    {
        private readonly Types.Options _options;
        private readonly Services.IUsersService _usersService;
        private readonly IDataContext _dataContext;

        public LoginCommandHandler(
            IOptions<Types.Options> options,
            Services.IUsersService usersService,
            IDataContext dataContext)
        {
            _options = options.Value;
            _usersService = usersService;
            _dataContext = dataContext;
        }

        public async Task<ResultModels.LoginViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var success = false;

            var user = await _dataContext.Users
                .FindByEmail(request.Username)
                .Select(u => new
                {
                    u.UserId,
                    u.CompanyId,
                    u.Password,
                    u.Admin,
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (user != null && _usersService.Authenticate(user.Password, request.Password))
            {
                if (_usersService.ShouldUpgrade(user.Password))
                {
                    var u = await _dataContext.Users.FindAsync(user.UserId);
                    u!.Password = _usersService.HashPassword(request.Password);
                    await _dataContext.SaveChangesAsync();
                }

                var userId = new ClaimsIdentity(new Claim[]
                {
                    new ("userid",user.UserId.ToString()),
                    new ("companyid",user.CompanyId.ToString()),
                    new (ClaimTypes.Role, user.Admin ? "Admin" : "User")
                }, request.AuthType);

                await request.SignInFunc(new(userId));
                success = true;
            }

            return new()
            {
                AllowRegistrations = _options.AllowNewAccountCreation,
                Success = success,
            };
        }
    }
}