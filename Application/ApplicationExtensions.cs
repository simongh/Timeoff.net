using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Timeoff.Application
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddTransient<IValidator<Account.LoginCommand>, Account.LoginCommandValidator>();
            services.AddTransient<IValidator<Account.RegisterCommand>, Account.RegisterCommandValidator>();
            services.AddTransient<IValidator<Account.ForgotPasswordComand>, Account.ForgotPasswordCommandValidator>();
            services.AddTransient<IValidator<Account.ResetPasswordCommand>, Account.ResetPasswordCommandValidator>();

            services.AddTransient<IValidator<PublicHoliday.UpdatePublicHolidayCommand>, PublicHoliday.UpdatePublicHolidayCommandValidator>();
            services.AddTransient<IValidator<Departments.UpdateDepartmentCommand>, Departments.UpdateDepartmentCommandValidator>();
            services.AddTransient<IValidator<Settings.UpdateSettingsCommand>, Settings.UpdateSettingsCommandValidator>();
            services.AddTransient<IValidator<Settings.UpdateLeaveTypesCommand>, Settings.UpdateLeaveTypesCommandValidator>();

            return services;
        }
    }
}