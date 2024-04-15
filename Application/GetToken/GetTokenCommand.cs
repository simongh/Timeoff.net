using MediatR;
using System.Security.Claims;
using Timeoff.Services;

namespace Timeoff.Application.GetToken
{
    public record GetTokenCommand : IRequest<ResultModels.TokenResult>
    {
        public ClaimsIdentity User { get; init; }
    }

    internal class GetTokenCommandHandler(Services.IUsersService usersService)
        : IRequestHandler<GetTokenCommand, ResultModels.TokenResult>
    {
        private readonly IUsersService _usersService = usersService;

        public async Task<ResultModels.TokenResult> Handle(GetTokenCommand request, CancellationToken cancellationToken)
        {
            return new()
            {
                Token = _usersService.CreateJwt(request.User),
                Expires = DateTimeOffset.UtcNow.AddMinutes(5),
            };
        }
    }
}