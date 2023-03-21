using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Commands
{
    public record UpdateSettingsCommand : Types.SettingsModel, IRequest<ResultModels.SettingsViewModel>, IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, ResultModels.SettingsViewModel>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;

        public UpdateSettingsCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
        }

        public async Task<ResultModels.SettingsViewModel> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult messages;

            if (request.Failures.IsValid())
            {
                var company = await _dataContext.Companies
                    .Where(c => c.CompanyId == _currentUserService.CompanyId)
                    .FirstOrDefaultAsync();

                if (company == null)
                {
                    messages = ResultModels.FlashResult.WithError("The current company was not found!");
                }
                else
                {
                    company.Name = request.Name;
                    company.CarryOver = request.CarryOver;
                    company.ShareAllAbsences = request.ShowHoliday;
                    company.IsTeamViewHidden = request.HideTeamView;
                    company.DateFormat = request.DateFormat;
                    company.TimeZone = request.TimeZone;
                    company.Country = request.Country;

                    await _dataContext.SaveChangesAsync();
                    messages = ResultModels.FlashResult.Success("Company details updated");
                }
            }
            else
                messages = request.Failures.ToFlashResult();

            var result = await _dataContext.GetSettingsAsync(_currentUserService.CompanyId);
            result.Result = messages;

            return result;
        }
    }
}