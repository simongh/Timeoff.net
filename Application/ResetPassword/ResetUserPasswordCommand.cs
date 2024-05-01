using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.ResetPassword
{
    public record ResetUserPasswordCommand : IRequest
    {
        public int Id { get; init; }
    }

    internal class ResetUserPasswordCommandHandler(
        IDataContext dataContext,
        Services.IUsersService usersService,
        Services.IEmailTemplateService emailTemplateService)
        : IRequestHandler<ResetUserPasswordCommand>
    {
        private readonly IDataContext _dataContext = dataContext;
        private readonly Services.IUsersService _usersService = usersService;
        private readonly Services.IEmailTemplateService _emailTemplateService = emailTemplateService;

        public async Task Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .Where(u => u.UserId == request.Id)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            user.Password = "";
            user.Token = _usersService.Token();
            _dataContext.EmailAudits.Add(_emailTemplateService.ForgotPassword(user));
            await _dataContext.SaveChangesAsync();
        }
    }
}