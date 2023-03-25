using HandlebarsDotNet;
using Timeoff.Services;

namespace Timeoff
{
    internal static class EmailExtensions
    {
        private static HandlebarsTemplate<object, object>? _confirmRegistration;
        private static HandlebarsTemplate<object, object>? _resetPassword;
        private static HandlebarsTemplate<object, object>? _forgotPassword;

        public static Entities.EmailAudit ConfirmRegistration(this IEmailTemplateService templateService, Entities.User user)
        {
            _confirmRegistration ??= templateService.CreateWrapper("ConfirmRegistration");

            var content = _confirmRegistration(new
            {
                user.FirstName,
                templateService.Options.SiteUrl,
            });

            return templateService.Wrap(content, user);
        }

        public static Entities.EmailAudit ResetPassword(this IEmailTemplateService templateService, Entities.User user)
        {
            _resetPassword ??= templateService.CreateWrapper("ResetPassword");
            var content = _resetPassword(new
            {
                user.FirstName,
                templateService.Options.SiteUrl,
            });

            return templateService.Wrap(content, user);
        }

        public static Entities.EmailAudit ForgotPassword(this IEmailTemplateService templateService, Entities.User user)
        {
            _forgotPassword ??= templateService.CreateWrapper("ForgotPassword");

            var content = _forgotPassword(new
            {
                user.FirstName,
                user.Company.LdapAuthEnabled,
                templateService.Options.SiteUrl,
                user.Token,
            });

            return templateService.Wrap(content, user);
        }
    }
}