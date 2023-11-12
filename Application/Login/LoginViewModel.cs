namespace Timeoff.Application.Login
{
    public record LoginViewModel
    {
        public bool AllowRegistrations { get; init; }

        public ResultModels.FlashResult? Result { get; init; }

        public bool Success { get; init; }
    }
}