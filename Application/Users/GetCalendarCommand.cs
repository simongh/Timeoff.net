using MediatR;

namespace Timeoff.Application.Users
{
    public record GetCalendarCommand : IRequest<CalendarViewModel>
    {
        public int Id { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;
    }

    public class GetCalendarComamndHandler : IRequestHandler<GetCalendarCommand, CalendarViewModel>
    {
        public Task<CalendarViewModel> Handle(GetCalendarCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CalendarViewModel());
        }
    }
}