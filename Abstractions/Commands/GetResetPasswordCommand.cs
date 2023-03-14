using MediatR;

namespace Timeoff.Commands
{
    public record GetResetPasswordCommand : IRequest<ResultModels.ResetPasswordViewModel>
    {
        public string? T { get; init; }
    }

    internal class GetResetPasswordCommandHandler : IRequestHandler<GetResetPasswordCommand, ResultModels.ResetPasswordViewModel>
    {
        private readonly Services.ICurrentUserService _currentUserService;

        public GetResetPasswordCommandHandler(Services.ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public Task<ResultModels.ResetPasswordViewModel> Handle(GetResetPasswordCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult? error = null;
            if (string.IsNullOrEmpty(request.T) && !_currentUserService.IsAuthenticated)
                error = ResultModels.FlashResult.WithError("Unknown reset password link, please submit request again");

            return Task.FromResult(new ResultModels.ResetPasswordViewModel
            {
                ShowCurrent = _currentUserService.IsAuthenticated,
                Token = request.T,
                Result = error,
            });
        }
    }
}