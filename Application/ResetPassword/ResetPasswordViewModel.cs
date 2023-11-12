namespace Timeoff.Application.ResetPassword
{
    public record ResetPasswordViewModel
    {
        public bool ShowCurrent { get; init; }

        public string? Token { get; init; }

        public ResultModels.FlashResult? Result { get; init; }
    }
}