using HandlebarsDotNet;
using Microsoft.Extensions.Options;

namespace Timeoff.Services
{
    internal class EmailTemplateService : IEmailTemplateService
    {
        private Lazy<HandlebarsTemplate<object, object>> _wrapper;
        private Lazy<HandlebarsTemplate<object, object>> _forgotPassword;
        private readonly Types.Options _options;

        public EmailTemplateService(IOptions<Types.Options> options)
        {
            _options = options.Value;
            _wrapper = new(() => CreateWrapper("wrapper"));
            _forgotPassword = new(() => CreateWrapper("ForgotPassword"));
        }

        private string GetTemplate(string name)
        {
            var file = Path.Combine(_options.Email.Templates, name + ".hbs");

            return File.ReadAllText(file);
        }

        private HandlebarsTemplate<object, object> CreateWrapper(string templateName)
        {
            var template = Handlebars.Create();
            template.RegisterHelper("concatenate", (writer, context, parameters) =>
            {
                writer.WriteSafeString(string.Join("", parameters));
            });
            template.RegisterTemplate("link", GetTemplate("link"));

            return template.Compile(GetTemplate(templateName));
        }

        private Entities.EmailAudit Wrap(string content, Entities.User user)
        {
            var parts = content.Split("\r\n=====\r\n");
            var body = _wrapper.Value(new
            {
                subject = parts[0],
                body = parts[1],
                siteUrl = _options.SiteUrl
            });

            return new Entities.EmailAudit
            {
                Email = user.Email,
                Body = body,
                Subject = parts[0],
                CreatedAt = DateTimeOffset.UtcNow,
                User = user,
                Company = user.Company,
            };
        }

        public Entities.EmailAudit ForgotPassword(Entities.User user)
        {
            var content = _forgotPassword.Value(new
            {
                user.Fullname,
                user.Company.LdapAuthEnabled,
                _options.SiteUrl,
                user.Token,
            });

            return Wrap(content, user);
        }
    }
}