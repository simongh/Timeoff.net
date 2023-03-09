using Microsoft.Extensions.Options;

namespace Timeoff.Services
{
    internal class EmailService
    {
        private readonly Types.Options _options;

        public EmailService(IOptions<Types.Options> options)
        {
            _options = options.Value;
        }

        public async Task Send(Entities.EmailAudit email)
        {
            if (!_options.Email.SendEmails)
                return;
        }
    }
}