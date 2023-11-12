using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Absences
{
    internal static class Extensions
    {
        public static async Task<AbsencesViewModel> GetAbsencesAync(this IDataContext dataContext, int companyId, int userId)
        {
            var user = await dataContext.Users
                .Where(u => u.CompanyId == companyId && u.UserId == userId)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.TeamId,
                    u.IsActivated,
                    u.EndDate,
                    u.Team.IsAccrued,
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException();
            }

            return new()
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TeamId = user.TeamId,
                IsActive = user.IsActivated && (user.EndDate == null || user.EndDate > DateTime.Today),
                IsAccrued = user.IsAccrued,
                Summary = await dataContext.GetAllowanceAsync(userId, DateTime.Today.Year),
                LeaveRequested = await dataContext.Leaves.GetRequested(userId, DateTime.Today.Year),
            };
        }
    }
}