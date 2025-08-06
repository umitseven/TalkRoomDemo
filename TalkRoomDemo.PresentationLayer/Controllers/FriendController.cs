using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
            int userId =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var friendList = await _friendService.TGetFriendsByUserId(userId);

            return PartialView("_FriendListPartial", friendList);
        }
        public async Task<IActionResult> Notify()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _hubContext.Clients.User(userId).SendAsync("ReceiveFriendListUpdate");
            return Ok();
        }
    }
}
