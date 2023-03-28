using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Timeoff.Application.Users
{
    public record ResetPasswordCommand : IRequest<DetailsViewModel?>
    {
        public int Id { get; init; }
    }

    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, DetailsViewModel?>
    {
        private readonly IDataContext _dataContext;
        private readonly Services.ICurrentUserService _currentUserService;
        private readonly Services.IUsersService _usersService;
        private readonly Services.IEmailTemplateService _emailTemplateService;

        public ResetPasswordCommandHandler(
            IDataContext dataContext,
            Services.ICurrentUserService currentUserService,
            Services.IUsersService usersService,
            Services.IEmailTemplateService emailTemplateService)
        {
            _dataContext = dataContext;
            _currentUserService = currentUserService;
            _usersService = usersService;
            _emailTemplateService = emailTemplateService;
        }

        public async Task<DetailsViewModel?> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .Where(u => u.CompanyId == _currentUserService.CompanyId && u.UserId == request.Id)
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException();

            user.Password = "";
            user.Token = _usersService.Token();
            _dataContext.EmailAudits.Add(_emailTemplateService.ForgotPassword(user));
            await _dataContext.SaveChangesAsync();

            var result = await _dataContext.GetUserDetailsAsync(_currentUserService.CompanyId, request.Id);
            result!.Messages = ResultModels.FlashResult.Success("Password reset");

            return result;
        }
    }
}