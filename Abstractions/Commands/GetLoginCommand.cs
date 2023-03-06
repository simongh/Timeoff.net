using MediatR;
using Microsoft.Extensions.Options;

namespace Timeoff.Commands
{
    public class GetLoginCommand : IRequest<ResultModels.LoginViewModel>
    { }

    internal class GetLoginCommandHandler : IRequestHandler<GetLoginCommand, ResultModels.LoginViewModel>
    {
        private readonly Types.Options _options;

        public GetLoginCommandHandler(IOptions<Types.Options> options)
        {
            _options = options.Value;
        }

        public Task<ResultModels.LoginViewModel> Handle(GetLoginCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ResultModels.LoginViewModel
            {
                AllowRegistrations = _options.AllowNewAccountCreation,
                Success = true,
            });
        }
    }
}