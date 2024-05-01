using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.UserDetails
{
    internal static class Extensions
    {
        public static async Task<DetailsViewModel?> GetUserDetailsAsync(this IDataContext dataContext, int userId)
        {
            var model = await dataContext.Users
                .Where(u => u.UserId == userId)
                .Select(u => new
                {
                    User = new DetailsViewModel
                    {
                        Id = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        StartDate = u.StartDate,
                        EndDate = u.EndDate,
                        AutoApprove = u.AutoApprove,
                        IsActive = u.IsActivated,
                        IsAdmin = u.IsAdmin,
                        Team = u.TeamId,
                        Email = u.Email,
                        //CompanyName = u.Company.Name,
                        //DateFormat = u.Company.DateFormat,
                        //Teams = u.Company.Teams
                        //.OrderBy(d => d.Name)
                        //.Select(d => new ResultModels.ListItem
                        //{
                        //    Id = d.TeamId,
                        //    Value = d.Name,
                        //}),
                    },
                    UserSchedule = u.Schedule,
                    CompanySchedule = u.Company.Schedule
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                throw new NotFoundException();
            }

            return model.User with
            {
                Schedule = (model.UserSchedule ?? model.CompanySchedule).ToModel(),
                ScheduleOverride = model.UserSchedule != null,
            };
        }
    }
}