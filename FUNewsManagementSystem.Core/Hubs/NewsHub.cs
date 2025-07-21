using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FUNewsManagementSystem.Hubs
{
    public class NewsHub : Hub
    {
        // Lưu ánh xạ giữa userId và connectionId
        public static Dictionary<string, string> UserConnectionMap = new();

        // Khi client kết nối
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                lock (UserConnectionMap)
                {
                    UserConnectionMap[userId] = Context.ConnectionId;
                }
            }

            return base.OnConnectedAsync();
        }

        // Khi client ngắt kết nối
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                lock (UserConnectionMap)
                {
                    UserConnectionMap.Remove(userId);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        // Hàm hỗ trợ client lấy connectionId (dùng trong Create/Edit.cshtml)
        public Task<string> GetConnectionId()
        {
            return Task.FromResult(Context.ConnectionId);
        }
    }
}
