using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    public record UpdatePublicHolidayCommand : IRequest<ResultModels.ApiResult>, Commands.IValidated
    {
        public PublicHolidayRequest[] PublicHolidays { get; init; } = null!;

        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdatePublicHolidayCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService,
        Services.IDaysCalculator adjuster)
        : IRequestHandler<UpdatePublicHolidayCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly Services.IDaysCalculator _adjuster = adjuster;
        private List<(DateTime original, DateTime modified)> _changes = [];

        public async Task<ResultModels.ApiResult> Handle(UpdatePublicHolidayCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures.IsValid())
            {
                Insert(request);
                await UpdateAsync(request);

                await _dataContext.SaveChangesAsync();

                if (_changes.Any())
                    await _adjuster.AdjustForHolidaysAsync(_changes);
            }

            return new()
            {
                Errors = request.Failures?.Select(e => e.ErrorMessage) ?? []
            };
        }

        private void Insert(UpdatePublicHolidayCommand request)
        {
            var toAdd = request.PublicHolidays.Where(h => h.Id == null);

            if (!toAdd.Any())
                return;

            foreach (var item in toAdd)
            {
                _dataContext.Calendar.Add(new()
                {
                    Date = item.Date,
                    Name = item.Name,
                    IsHoliday = true,
                    CompanyId = _currentUserService.CompanyId,
                });

                _changes.Add((item.Date, item.Date));
            }
        }

        private async Task UpdateAsync(UpdatePublicHolidayCommand request)
        {
            var ids = request.PublicHolidays
                .Where(h => h.Id.HasValue)
                .Select(h => h.Id!.Value);

            if (!ids.Any())
                return;

            var items = await _dataContext.Calendar
                .Where(h => ids.Contains(h.CalendarId) && h.IsHoliday)
                .ToArrayAsync();

            foreach (var item in items)
            {
                var m = request.PublicHolidays.First(h => h.Id == item.CalendarId);

                if (m.Date != item.Date)
                    _changes.Add((item.Date, m.Date));

                item.Date = m.Date;
                item.Name = m.Name;
            }
        }
    }
}