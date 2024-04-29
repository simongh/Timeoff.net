using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Timeoff.Services
{
    internal class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirstValue("userid");
        }
    }
}