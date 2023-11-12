using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.IntegrationApi
{
    internal static class Extensions
    {
        public static async Task<IntegrationApiViewModel> GetIntegrationApiAsync(this IDataContext dataContext, int companyId)
        {
            return await dataContext.Companies
                .Where(c => c.CompanyId == companyId)
                .Select(c => new IntegrationApiViewModel
                {
                    Name = c.Name,
                    ApiKey = c.IntegrationApiToken.ToString(),
                    Enabled = c.IntegrationApiEnabled,
                })
                .FirstAsync();
        }
    }
}