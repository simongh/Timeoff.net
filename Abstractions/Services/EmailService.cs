using Microsoft.Extensions.Options;

namespace Timeoff.Services
{
    internal class EmailService(IOptions<Types.Options> options)
    {
        private readonly Types.Options _options = options.Value;

        public async Task SendAsync(Entities.EmailAudit email)
        {
            if (!_options.Email.SendEmails)
                return;
        }
    }
}