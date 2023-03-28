using MediatR;

namespace Timeoff.Application.Users
{
    public record GetCalendarCommand : IRequest<CalendarViewModel>
    {
        public int Id { get; init; }

        public int Year { get; init; } = DateTime.Today.Year;
    }
}