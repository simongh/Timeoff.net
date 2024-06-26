﻿using HandlebarsDotNet;
using Microsoft.Extensions.Options;

namespace Timeoff.Services
{
    internal class EmailTemplateService : IEmailTemplateService
    {
        private static HandlebarsTemplate<object, object> _wrapper = null!;
        private static SemaphoreSlim _lock = new(1);

        public Types.Options Options { get; }

        public EmailTemplateService(IOptions<Types.Options> options)
        {
            Options = options.Value;
            ConfigureWrapper();
        }

        private async void ConfigureWrapper()
        {
            if (_wrapper != null)
                return;

            try
            {
                await _lock.WaitAsync();

                if (_wrapper != null)
                    return;

                _wrapper = CreateWrapper("wrapper");
            }
            finally
            {
                _lock.Release();
            }
        }

        private string GetTemplate(string name)
        {
            var file = Path.Combine(Options.Email.Templates, name + ".hbs");

            return File.ReadAllText(file);
        }

        public HandlebarsTemplate<object, object> CreateWrapper(string templateName)
        {
            var template = Handlebars.Create();
            template.RegisterHelper("concatenate", (writer, context, parameters) =>
            {
                writer.WriteSafeString(string.Join("", parameters));
            });
            template.RegisterTemplate("link", GetTemplate("link"));

            return template.Compile(GetTemplate(templateName));
        }

        public Entities.EmailAudit Wrap(string content, Entities.User user)
        {
            var parts = content.Split("\r\n=====\r\n");
            var body = _wrapper(new
            {
                subject = parts[0],
                body = parts[1],
                siteUrl = Options.SiteUrl
            });

            return new Entities.EmailAudit
            {
                Email = user.Email,
                Body = body,
                Subject = parts[0],
                CreatedAt = DateTime.UtcNow,
                User = user,
                CompanyId = user.CompanyId,
            };
        }
    }
}