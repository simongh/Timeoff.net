using MediatR;

namespace Timeoff.Application.ResetPassword
{
    public record GetResetPasswordCommand : IRequest<ResetPasswordViewModel>
    {
        public string? T { get; init; }
    }

    internal class GetResetPasswordCommandHandler : IRequestHandler<GetResetPasswordCommand, ResetPasswordViewModel>
    {
        private readonly Services.ICurrentUserService _currentUserService;

        public GetResetPasswordCommandHandler(Services.ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public Task<ResetPasswordViewModel> Handle(GetResetPasswordCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult? error = null;
            if (string.IsNullOrEmpty(request.T) && !_currentUserService.IsAuthenticated)
                error = ResultModels.FlashResult.WithError("Unknown reset password link, please submit request again");

            return Task.FromResult(new ResetPasswordViewModel
            {
                ShowCurrent = _currentUserService.IsAuthenticated,
                Token = request.T,
                Result = error,
            });
        }
    }
}