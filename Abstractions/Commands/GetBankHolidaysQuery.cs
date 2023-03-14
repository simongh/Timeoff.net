using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record GetBankHolidaysQuery : IRequest<ResultModels.BankHolidaysViewModel>
    {
        public int Year { get; init; } = DateTime.Today.Year;
    }

    internal class GetBankHolidaysQueryHandler : IRequestHandler<GetBankHolidaysQuery, ResultModels.BankHolidaysViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public GetBankHolidaysQueryHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.BankHolidaysViewModel> Handle(GetBankHolidaysQuery request, CancellationToken cancellationToken)
        {
            var company = await _dataContext.Companies
                .FindById(_currentUserService.CompanyId)
                .Select(c => new
                {
                    c.Name,
                    c.DateFormat,
                    BankHolidays = c.BankHolidays
                        .Where(h => h.Date.Year == request.Year)
                        .Select(h => new ResultModels.BankHolidayResult
                        {
                            Id = h.BankHolidayId,
                            Date = h.Date,
                            Name = h.Name,
                        }),
                })
                .FirstAsync();

            var noLeave = Enumerable.Empty<Entities.Leave>();
            var startDate = new DateTime(request.Year, 1, 1);
            var calendar = new List<ResultModels.CalendarMonthResult>();

            for (int i = 0; i < 12; i++)
            {
                calendar.Add(ResultModels.CalendarMonthResult.FromDate(
                    startDate.AddMonths(i),
                    noLeave,
                    company.BankHolidays.Where(h => h.Date.Month == startDate.Month)));
            }

            return new()
            {
                CompanyName = company.Name,
                DateFormat = company.DateFormat,
                CurrentYear = request.Year,
                Calendar = calendar,
                BankHolidays = company.BankHolidays.ToArray(),
            };
        }
    }
}