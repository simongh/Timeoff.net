using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record UpdateBankHolidayCommand : IRequest<ResultModels.PublicHolidaysViewModel>, IValidated
    {
        public RequestModels.PublicHolidayRequest[]? BankHolidays { get; init; }

        public RequestModels.PublicHolidayRequest? Add { get; init; }

        public int Year { get; init; }
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class NewBankHolidayCommandHandler : IRequestHandler<UpdateBankHolidayCommand, ResultModels.PublicHolidaysViewModel>
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

        public async Task<ResultModels.PublicHolidaysViewModel> Handle(UpdateBankHolidayCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures.IsValid())
            {
                Insert(request);
                await UpdateAsync(request);

                await _dataContext.SaveChangesAsync();
            }

            var result = await _dataContext.Companies.GetPublicHolidaysAsync(_currentUserService.CompanyId, request.Year);
            result.Result = new()
            {
                Errors = request.Failures?.Select(e => e.ErrorMessage)
            };

            return result;
        }

        private void Insert(UpdateBankHolidayCommand request)
        {
            if (request.Add == null)
            {
                return;
            }

            _dataContext.BankHolidays.Add(new()
            {
                Date = request.Add.Date,
                Name = request.Add.Name,
                CompanyId = _currentUserService.CompanyId,
            });
        }

        private async Task UpdateAsync(UpdateBankHolidayCommand request)
        {
            if (request.BankHolidays == null)
                return;

            var ids = request.BankHolidays
                .Where(h => h.Id.HasValue)
                .Select(h => h.Id!.Value);

            var items = await _dataContext.BankHolidays
                .Where(h => ids.Contains(h.PublicHolidayId))
                .ToArrayAsync();

            foreach (var item in items)
            {
                var m = request.BankHolidays.First(h => h.Id == item.PublicHolidayId);
                item.Date = m.Date;
                item.Name = m.Name;
            }
        }
    }
}