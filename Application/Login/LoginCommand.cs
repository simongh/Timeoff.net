using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Timeoff.Application.Login
{
    public record LoginCommand : IRequest<ResultModels.TokenResult>, Commands.IValidated
    {
        public string? Username { get; init; }

        public string? Password { get; init; }

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class LoginCommandHandler(
        IOptions<Types.Options> options,
        Services.IUsersService usersService,
        Services.ICurrentUserService currentUserService,
        IDataContext dataContext)
        : IRequestHandler<LoginCommand, ResultModels.TokenResult>
    {
        private readonly Types.Options _options = options.Value;
        private readonly Services.IUsersService _usersService = usersService;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly IDataContext _dataContext = dataContext;

        public async Task<ResultModels.TokenResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures == null)
            {
                var user = await _dataContext.Users
                .FindByEmail(request.Username)
                .Where(u => u.IsActivated)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(cancellationToken);

                if (user != null && _usersService.Authenticate(user.Password, request.Password))
                {
                    if (_usersService.ShouldUpgrade(user.Password))
                    {
                        user.Password = _usersService.HashPassword(request.Password!);
                    }
                    user.Token = null;
                    user.IsActivated = true;
                    await _dataContext.SaveChangesAsync();

                    var userId = new ClaimsIdentity(
                    [
                        new ("userid",user.UserId.ToString()),
                        new ("companyid",user.CompanyId.ToString()),
                        new ("dateformat",user.Company.DateFormat),
                        new ("showTeamView",(!user.Company.IsTeamViewHidden).ToString()),
                        new (ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                    ], _currentUserService.AuthenticationScheme);

                    await _currentUserService.SignInAsync(new(userId));

                    return user.ToResult(_usersService.CreateJwt(userId));
                }
            }

            return new()
            {
                Success = false,
            };
        }
    }
}