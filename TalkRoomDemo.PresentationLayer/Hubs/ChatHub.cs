using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework.Profiler;
using System.Security.Claims;

namespace TalkRoomDemo.PresentationLayer.Hubs
{
    public class ChatHub :Hub
    {
        public async Task SendMessage(string user,string profileUrl, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, profileUrl, message);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"Kullanıcı bağlandı: {userId}");
            await base.OnConnectedAsync();
        }



    }
}
    