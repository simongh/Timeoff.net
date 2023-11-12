﻿using FluentValidation;
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

            services.AddTransient<IValidator<Login.LoginCommand>, Login.LoginCommandValidator>();
            services.AddTransient<IValidator<Register.RegisterCommand>, Register.RegisterCommandValidator>();
            services.AddTransient<IValidator<ForgotPassword.ForgotPasswordComand>, ForgotPassword.ForgotPasswordCommandValidator>();
            services.AddTransient<IValidator<ResetPassword.ResetPasswordCommand>, ResetPassword.ResetPasswordCommandValidator>();

            services.AddTransient<IValidator<PublicHolidays.UpdatePublicHolidayCommand>, PublicHolidays.UpdatePublicHolidayCommandValidator>();
            services.AddTransient<IValidator<TeamDetails.UpdateTeamCommand>, TeamDetails.UpdateTeamCommandValidator>();

            services.AddTransient<IValidator<Settings.UpdateSettingsCommand>, Settings.UpdateSettingsCommandValidator>();
            services.AddTransient<IValidator<Settings.UpdateLeaveTypesCommand>, Settings.UpdateLeaveTypesCommandValidator>();

            services.AddTransient<Validators.UserDetailsBaseValidator>();
            services.AddTransient<IValidator<UserDetails.UpdateUserCommand>, UserDetails.UpdateUserCommandValidator>();
            services.AddTransient<IValidator<CreateUser.CreateCommand>, CreateUser.CreateCommandValidator>();

            services.AddTransient<IValidator<BookAbsence.BookCommand>, BookAbsence.BookCommandValidator>();

            return services;
        }
    }
}