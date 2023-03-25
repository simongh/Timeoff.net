namespace Timeoff.Application.Account
{
    public record ResetPasswordViewModel
    {
        public bool ShowCurrent { get; init; }

        public string? Token { get; init; }

        public ResultModels.FlashResult? Result { get; init; }
    }
}