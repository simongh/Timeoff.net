﻿using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record DeleteHolidayCommand : IRequest<ResultModels.PublicHolidaysViewModel>
    {
        public int Id { get; init; }

        public int Year { get; init; }
    }

    internal class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, ResultModels.PublicHolidaysViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public DeleteHolidayCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.PublicHolidaysViewModel> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
        {
            var holiday = await _dataContext.BankHolidays
                .FirstOrDefaultAsync(h => h.PublicHolidayId == request.Id && h.CompanyId == _currentUserService.CompanyId);

            ResultModels.FlashResult result;
            if (holiday == null)
            {
                result = ResultModels.FlashResult.WithError("Unabled to find holiday");
            }
            else
            {
                _dataContext.BankHolidays.Remove(holiday);
                await _dataContext.SaveChangesAsync();
                result = ResultModels.FlashResult.Success("Holiday was successfully removed");
            }

            var vm = await _dataContext.Companies.GetPublicHolidaysAsync(_currentUserService.CompanyId, request.Year);
            vm.Result = result;

            return vm;
        }
    }
}