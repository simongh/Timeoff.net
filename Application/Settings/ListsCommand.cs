using MediatR;

namespace Timeoff.Application.Settings
{
    public record ListsCommand : IRequest<ListsResult>
    { }

    internal class ListsCommandHandler : IRequestHandler<ListsCommand, ListsResult>
    {
        public Task<ListsResult> Handle(ListsCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ListsResult
            {
                Countries = Services.CountriesService.Countries,
                TimeZones = Services.TimeZoneService.TimeZones.Select(tz => new
                {
                    Id = tz.Id,
                    Name = tz.DisplayName
                }),
            });
        }
    }
}