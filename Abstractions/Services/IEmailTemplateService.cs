using HandlebarsDotNet;

namespace Timeoff.Services
{
    public interface IEmailTemplateService
    {
        Types.Options Options { get; }

        HandlebarsTemplate<object, object> CreateWrapper(string templateName);

        Entities.EmailAudit Wrap(string content, Entities.User user);
    }
}