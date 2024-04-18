using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Timeoff.Application.GetToken
{
    public record GetTokenCommand : IRequest<ResultModels.TokenResult>
    {
        public ClaimsIdentity User { get; init; } = null!;
    }

    internal class GetTokenCommandHandler(
        Services.IUsersService usersService,
        Services.ICurrentUserService currentUserService,
        IDataContext dataContext)
        : IRequestHandler<GetTokenCommand, ResultModels.TokenResult>
    {
        private readonly Services.IUsersService _usersService = usersService;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly IDataContext _dataContext = dataContext;

        public async Task<ResultModels.TokenResult> Handle(GetTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .Where(u => u.UserId == _currentUserService.UserId)
                .Include(u => u.Company)
                .FirstOrDefaultAsync(cancellationToken);

            if (user?.IsActive != true)
            {
                return new()
                {
                    Success = false,
                };
            }

            return user.ToResult(_usersService.CreateJwt(request.User));
        }
    }
}