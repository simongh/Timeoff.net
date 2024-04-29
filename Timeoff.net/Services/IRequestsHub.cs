namespace Timeoff.Services
{
    public interface IRequestsHub
    {
        Task AwaitingApproval(int count);
    }
}