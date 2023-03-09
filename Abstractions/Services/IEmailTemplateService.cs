namespace Timeoff.Services
{
    public interface IEmailTemplateService
    {
        Entities.EmailAudit ForgotPassword(Entities.User user);
    }
}