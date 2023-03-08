using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Timeoff
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsActions)
        {
            services.AddTransient<Services.IUsersService, Services.UsersService>();
            services.AddScoped<IDataContext, DataContext>();
            services.AddDbContext<DataContext>(optionsActions);

            return services;
        }

        public static void Migrate(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
        }
    }
}