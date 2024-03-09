using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.PublicHolidays
{
    public record DeleteHolidayCommand : IRequest<PublicHolidaysViewModel>
    {
        public int Id { get; init; }
    }

    internal class DeleteHolidayCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService,
        Services.IDaysCalculator adjuster)
        : IRequestHandler<DeleteHolidayCommand, PublicHolidaysViewModel>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;
        private readonly Services.IDaysCalculator _adjuster = adjuster;

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

            return new()
            {
                Result = result,
            };
        }
    }
}