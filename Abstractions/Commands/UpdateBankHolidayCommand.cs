using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record UpdateBankHolidayCommand : IRequest<ResultModels.BankHolidaysViewModel>, IValidated
    {
        public RequestModels.BankHolidayRequest[] BankHolidays { get; init; }

        public int Year { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class NewBankHolidayCommandHandler : IRequestHandler<UpdateBankHolidayCommand, ResultModels.BankHolidaysViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public NewBankHolidayCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.BankHolidaysViewModel> Handle(UpdateBankHolidayCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures == null)
            {
                Insert(request);
                await UpdateAsync(request);

                await _dataContext.SaveChangesAsync();
            }

            var result = await _dataContext.Companies.GetBankHolidaysAsync(_currentUserService.CompanyId, request.Year);
            result.Result = new()
            {
                Errors = request.Failures?.Select(e => e.ErrorMessage)
            };

            return result;
        }

        private void Insert(UpdateBankHolidayCommand request)
        {
            var items = request.BankHolidays
                .Where(h => h.Id == null);

            if (!items.Any())
                return;

            _dataContext.BankHolidays.AddRange(items.Select(h => new Entities.BankHoliday
            {
                Date = h.Date,
                Name = h.Name,
                CompanyId = _currentUserService.CompanyId,
            }));
        }

        private async Task UpdateAsync(UpdateBankHolidayCommand request)
        {
            var ids = request.BankHolidays
                .Where(h => h.Id.HasValue)
                .Select(h => h.Id!.Value);

            var items = await _dataContext.BankHolidays
                .Where(h => ids.Contains(h.BankHolidayId))
                .ToArrayAsync();

            foreach (var item in items)
            {
                var m = request.BankHolidays.First(h => h.Id == item.BankHolidayId);
                item.Date = m.Date;
                item.Name = m.Name;
            }
        }
    }
}