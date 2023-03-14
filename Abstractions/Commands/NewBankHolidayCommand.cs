using MediatR;

namespace Timeoff.Commands
{
    public record NewBankHolidayCommand : IRequest<ResultModels.BankHolidaysViewModel>
    {
        public RequestModels.BankHolidayRequest[] Holidays { get; init; }

        public int Year { get; init; }
    }

    internal class NewBankHolidayCommandHandler : IRequestHandler<NewBankHolidayCommand, ResultModels.BankHolidaysViewModel>
    {
        public NewBankHolidayCommandHandler()
        { }

        public Task<ResultModels.BankHolidaysViewModel> Handle(NewBankHolidayCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}