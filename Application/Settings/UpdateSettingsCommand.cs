using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record UpdateSettingsCommand : SettingsModel, IRequest<ResultModels.ApiResult>, Commands.IValidated
    {
        public IEnumerable<ValidationFailure>? Failures { get; set; }
    }

    internal class UpdateSettingsCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<UpdateSettingsCommand, ResultModels.ApiResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<ResultModels.ApiResult> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
        {
            if (request.Failures.IsValid())
            {
                var company = await _dataContext.Companies
                    .Where(c => c.CompanyId == _currentUserService.CompanyId)
                    .FirstOrDefaultAsync();

                if (company == null)
                {
                    return new()
                    {
                        Errors = ["The current company was not found!"]
                    };
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

                    return new();
                }
            }
            else
                return new()
                {
                    Errors = request.Failures.Select(v => v.ErrorMessage)
                };
        }
    }
}