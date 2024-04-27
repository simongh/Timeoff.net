using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Timeoff.Application;

namespace Timeoff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .Configure<Types.Options>(builder.Configuration.GetSection("timeoff"))
                .AddApplicationServices(options =>
                {
                    options.UseSqlite(builder.Configuration.GetConnectionString("timeoff"));
                })
                .ConfigureApplication()
                .AddWebServices()
                .ConfigureJson();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.LogoutPath = "/logout";

                    options.Events.OnRedirectToAccessDenied =
                        options.Events.OnRedirectToLogin = c =>
                            {
                                c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                return Task.CompletedTask;
                            };
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["timeoff:siteUrl"],
                        ValidAudience = builder.Configuration["timeoff:siteUrl"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["timeoff:secret"]!))
                    };
                });

            builder.Services.AddControllersWithViews();
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //});

            builder.Services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build())
                .AddPolicy("token", policy => policy
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build())
                .AddPolicy("cookies", policy => policy
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser());

            var app = builder.Build();

            app.Services.Migrate();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers().RequireAuthorization();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}