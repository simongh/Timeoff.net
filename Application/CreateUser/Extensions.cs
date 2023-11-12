using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.CreateUser
{
    internal static class Extensions
    {
        public static async Task<CreateViewModel> GetCreateViewModelAsync(this IDataContext dataContext, int companyId)
        {
            return await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new CreateUser.CreateViewModel
                {
                    CompanyName = c.Name,
                    DateFormat = c.DateFormat,
                    Teams = c.Teams
                        .OrderBy(t => t.Name)
                        .Select(t => new ResultModels.ListItem
                        {
                            Id = t.TeamId,
                            Value = t.Name,
                        })
                })
                .FirstAsync();
        }
    }
}