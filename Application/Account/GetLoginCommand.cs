using MediatR;
using Microsoft.Extensions.Options;

namespace Timeoff.Application.Account
{
    public class GetLoginCommand : IRequest<LoginViewModel>
    { }

    internal class GetLoginCommandHandler : IRequestHandler<GetLoginCommand, LoginViewModel>
    {
        private readonly Types.Options _options;

        public GetLoginCommandHandler(IOptions<Types.Options> options)
        {
            _options = options.Value;
        }

        public Task<LoginViewModel> Handle(GetLoginCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new LoginViewModel
            {
                AllowRegistrations = _options.AllowNewAccountCreation,
                Success = true,
            });
        }
    }
}