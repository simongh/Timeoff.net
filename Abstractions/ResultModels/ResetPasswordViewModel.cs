namespace Timeoff.ResultModels
{
    public record ResetPasswordViewModel
    {
        public bool ShowCurrent { get; init; }

        public string? Token { get; init; }

        public FlashResult? Result { get; init; }
    }
}