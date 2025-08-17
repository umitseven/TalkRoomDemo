using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Security.Claims;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.PresentationLayer.Hubs;

namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;
        private readonly IHubContext<ChatHub> _hubContext;
      

        public FriendController(IFriendService friendService, IHubContext<ChatHub> hubContext)
        {
            
            _friendService = friendService;
            _hubContext = hubContext;
        }
        public async Task <IActionResult> GetFriendPartial()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            int userId = int.Parse(userIdStr);
            var friend = await _friendService.TGetFriendsByUserId(userId);
            
            foreach(var f in friend)
            {
                f.IsOnline = ChatHub.OnlineUsers.ContainsKey(f.FriendId.ToString());
            }

            return PartialView("_FriendListPartial", friend);
        }
        public async Task<IActionResult> Notify()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var friendList = await _friendService.TGetFriendsByUserId(int.Parse(userId));
            await _hubContext.Clients.User(userId).SendAsync("ReceiveFriendListUpdate");
            return Ok();
        }
        
    }
}
