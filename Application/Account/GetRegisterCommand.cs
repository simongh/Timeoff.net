using MediatR;
using Microsoft.Extensions.Options;

namespace Timeoff.Application.Account
{
    public class GetRegisterCommand : IRequest<RegisterViewModel?>
    {
    }

    internal class GetRegisterCommandHandler : IRequestHandler<GetRegisterCommand, RegisterViewModel?>
    {
        private readonly Types.Options _options;

        public GetRegisterCommandHandler(
            IOptions<Types.Options> options)
        {
            _options = options.Value;
        }

        public async Task<RegisterViewModel?> Handle(GetRegisterCommand request, CancellationToken cancellationToken)
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