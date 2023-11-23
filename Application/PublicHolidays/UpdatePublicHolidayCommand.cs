using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    public record UpdatePublicHolidayCommand : IRequest<PublicHolidaysViewModel>, Commands.IValidated
    {
        public PublicHolidayRequest[]? PublicHolidays { get; init; }

        public PublicHolidayRequest? Add { get; init; }

        public int Year { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdatePublicHolidayCommandHandler : IRequestHandler<UpdatePublicHolidayCommand, PublicHolidaysViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly Services.IDaysCalculator _adjuster;
        private List<(DateTime original, DateTime modified)> _changes = new();

        public UpdatePublicHolidayCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService,
            Services.IDaysCalculator adjuster)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
            _adjuster = adjuster;
        }

        public async Task<PublicHolidaysViewModel> Handle(UpdatePublicHolidayCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures.IsValid())
            {
                Insert(request);
                await UpdateAsync(request);

                await _dataContext.SaveChangesAsync();

                if (_changes.Any())
                    await _adjuster.AdjustForHolidaysAsync(_changes, _currentUserService.CompanyId);
            }

            var result = await _dataContext.Companies.GetPublicHolidaysAsync(_currentUserService.CompanyId, request.Year);
            result.Result = new()
            {
                Errors = request.Failures?.Select(e => e.ErrorMessage)
            };

            return result;
        }

        private void Insert(UpdatePublicHolidayCommand request)
        {
            if (request.Add == null)
            {
                return;
            }

            _dataContext.PublicHolidays.Add(new()
            {
                Date = request.Add.Date,
                Name = request.Add.Name,
                CompanyId = _currentUserService.CompanyId,
            });

            _changes.Add((request.Add.Date, request.Add.Date));
        }

        private async Task UpdateAsync(UpdatePublicHolidayCommand request)
        {
            if (request.PublicHolidays == null)
                return;

            var ids = request.PublicHolidays
                .Where(h => h.Id.HasValue)
                .Select(h => h.Id!.Value);

            var items = await _dataContext.PublicHolidays
                .Where(h => ids.Contains(h.PublicHolidayId))
                .ToArrayAsync();

            foreach (var item in items)
            {
                var m = request.PublicHolidays.First(h => h.Id == item.PublicHolidayId);

                if (m.Date != item.Date)
                    _changes.Add((item.Date, m.Date));

                item.Date = m.Date;
                item.Name = m.Name;
            }
        }
    }
}