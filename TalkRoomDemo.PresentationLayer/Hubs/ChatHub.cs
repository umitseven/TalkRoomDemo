using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework.Profiler;

namespace TalkRoomDemo.PresentationLayer.Hubs
{
    public class ChatHub :Hub
    {
        public async Task SendMessage(string user,string profileUrl, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, profileUrl, message);
        }
        public async Task NotifyFriendListUpdate(string userId)
        {
            await Clients.User(userId).SendAsync("ReceiveFriendListUpdate");
        }

    }
}
    