using MediatR;
using Microsoft.Extensions.Options;

namespace Timeoff.Commands
{
    public class GetRegisterCommand : IRequest<ResultModels.RegisterViewModel?>
    {
    }

    internal class GetRegisterCommandHandler : IRequestHandler<GetRegisterCommand, ResultModels.RegisterViewModel?>
    {
        private readonly Types.Options _options;

        public GetRegisterCommandHandler(
            IOptions<Types.Options> options)
        {
            _options = options.Value;
        }

        public async Task<ResultModels.RegisterViewModel?> Handle(GetRegisterCommand request, CancellationToken cancellationToken)
        {
            if (!_options.AllowNewAccountCreation)
                return null;

            return new()
            {
                TimeZone = TimeZoneInfo.Local.Id,
                Countries = Services.CountriesService.Countries,
                TimeZones = Services.TimeZoneService.TimeZones,
            };
        }
    }
}