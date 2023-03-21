﻿using FluentValidation;
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

            services.AddTransient<IValidator<Commands.LoginCommand>, Validators.LoginCommandValidator>();
            services.AddTransient<IValidator<Commands.RegisterCommand>, Validators.RegisterCommandValidator>();
            services.AddTransient<IValidator<Commands.ForgotPasswordComand>, Validators.ForgotPasswordCommandValidator>();
            services.AddTransient<IValidator<Commands.ResetPasswordCommand>, Validators.ResetPasswordCommandValidator>();
            services.AddTransient<IValidator<Commands.UpdateBankHolidayCommand>, Validators.UpdateBankHolidayCommandValidator>();
            services.AddTransient<IValidator<Commands.UpdateDepartmentCommand>, Validators.UpdateDepartmentCommandValidator>();
            services.AddTransient<IValidator<Commands.UpdateSettingsCommand>, Validators.UpdateSettingsCommandValidator>();
            services.AddTransient<IValidator<Commands.UpdateLeaveTypesCommand>, Validators.UpdateLeaveTypesCommandValidator>();

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