namespace Timeoff
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<Services.ICurrentUserService, Services.CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddMediatR(options =>
             {
                 options.RegisterServicesFromAssembly(typeof(IDataContext).Assembly);
             });

            return services;
        }
    }
}