using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    public record DeleteHolidayCommand : IRequest<PublicHolidaysViewModel>
    {
        public int Id { get; init; }

        public int Year { get; init; }
    }

    internal class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, PublicHolidaysViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly Services.IDaysCalculator _adjuster;

        public DeleteHolidayCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService,
            Services.IDaysCalculator adjuster)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
            _adjuster = adjuster;
        }

        public async Task<PublicHolidaysViewModel> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            var holiday = await _dataContext.PublicHolidays
                .FirstOrDefaultAsync(h => h.PublicHolidayId == request.Id && h.CompanyId == _currentUserService.CompanyId);

            ResultModels.FlashResult result;
            if (holiday == null)
            {
                result = ResultModels.FlashResult.WithError("Unabled to find holiday");
            }
            else
            {
                _dataContext.PublicHolidays.Remove(holiday);
                await _dataContext.SaveChangesAsync();

                await _adjuster.AdjustForHolidaysAsync(new[] { (holiday.Date, holiday.Date) }, _currentUserService.CompanyId);

                result = ResultModels.FlashResult.Success("Holiday was successfully removed");
            }

            var vm = await _dataContext.Companies.GetPublicHolidaysAsync(_currentUserService.CompanyId, request.Year);
            vm.Result = result;

            return vm;
        }
    }
}