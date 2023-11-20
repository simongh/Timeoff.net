namespace Timeoff.Services
{
    public interface IFlashMessageService
    {
        ResultModels.FlashResult? FindMessages();

        void SaveMessage(ResultModels.FlashResult? result);
    }
}