using MediatR;
using System.Security.Claims;

namespace Timeoff.Commands
{
    public record GetResetPasswordCommand : IRequest<ResultModels.ResetPasswordViewModel>
    {
        public string? T { get; init; }

        public ClaimsPrincipal? User { get; set; }
    }

    internal class GetResetPasswordCommandHandler : IRequestHandler<GetResetPasswordCommand, ResultModels.ResetPasswordViewModel>
    {
        public Task<ResultModels.ResetPasswordViewModel> Handle(GetResetPasswordCommand request, CancellationToken cancellationToken)
        {
            ResultModels.FlashResult? error = null;
            if (string.IsNullOrEmpty(request.T) && request.User?.Identity?.IsAuthenticated != true)
                error = ResultModels.FlashResult.WithError("Unknown reset password link, please submit request again");

            return Task.FromResult(new ResultModels.ResetPasswordViewModel
            {
                ShowCurrent = request.User?.Identity?.IsAuthenticated == true,
                Token = request.T,
                Result = error,
            });
        }
    }
}