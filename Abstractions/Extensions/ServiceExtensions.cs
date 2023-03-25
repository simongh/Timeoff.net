using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Timeoff
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsActions)
        {
            services.AddTransient<Services.IUsersService, Services.UsersService>();
            services.AddScoped<IDataContext, DataContext>();
            services.AddDbContext<DataContext>(optionsActions);
            services.AddSingleton<Services.IEmailTemplateService, Services.EmailTemplateService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviours.UnhandledExceptionBehaviour<,>));

            return services;
        }

        public static void Migrate(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
        }
    }
}