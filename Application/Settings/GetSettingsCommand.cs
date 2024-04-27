using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Settings
{
    public record GetSettingsCommand : IRequest<SettingsResult>
    {
    }

    internal class GetSettingsCommandHandler(
        IDataContext dataContext,
        Services.ICurrentUserService currentUserService)
        : IRequestHandler<GetSettingsCommand, SettingsResult>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.ICurrentUserService _currentUserService = currentUserService;

        public async Task<SettingsResult> Handle(GetSettingsCommand request, CancellationToken cancellationToken)
        {
            return await _dataContext.Companies
                .Where(c => c.CompanyId == _currentUserService.CompanyId)
                .Select(c => new SettingsResult
                {
                    Name = c.Name,
                    CarryOver = c.CarryOver,
                    Country = c.Country,
                    DateFormat = c.DateFormat,
                    TimeZone = c.TimeZone,
                    HideTeamView = c.IsTeamViewHidden,
                    ShowHoliday = c.ShareAllAbsences,
                    Schedule = c.Schedule.ToModel(),
                    LeaveTypes = c.LeaveTypes.ToModels(),
                })
                .FirstAsync();
        }
    }
}