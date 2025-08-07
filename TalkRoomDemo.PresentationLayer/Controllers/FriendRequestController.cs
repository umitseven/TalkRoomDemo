using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.EntityLayer.Concrete;
using TalkRoomDemo.PresentationLayer.Hubs;

namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class FriendRequestController : Controller
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFriendService _friendService;
        private readonly IHubContext<ChatHub> _hubContext;

        public FriendRequestController(IFriendRequestService friendRequestService, UserManager<AppUser> userManager, IFriendService friendService, IHubContext<ChatHub> hubContext)
        {
            _friendRequestService = friendRequestService;
            _userManager = userManager;
            _friendService = friendService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> AcceptInvite(string inviteCode)
        {
            if (string.IsNullOrEmpty(inviteCode))
                return BadRequest("Kod boş olamaz.");

            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdStr))
                return Unauthorized();

            int currentUserId = int.Parse(currentUserIdStr);

            var targetUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.FriendCode == inviteCode);

            if(targetUser == null)
                return NotFound("Davet Kodu Geçersiz.");

            if (targetUser.Id == currentUserId)
                return BadRequest("Kendine arkadaşlık isteği gönderemezsin");

            var friendRequest = new FriendRequest
            {
                SenderUserId = currentUserId,
                ReceiverUserId = targetUser.Id,
                SendAt = DateTime.Now,
                IsAccepted = true
            };

            await _friendRequestService.TInsertAsync(friendRequest);
            await _friendService.CreateFriendshipAsync(currentUserId, targetUser.Id);
            await _hubContext.Clients.User(currentUserId.ToString()).SendAsync("ReceiveFriendListUpdate");
            await _hubContext.Clients.User(targetUser.Id.ToString()).SendAsync("ReceiveFriendListUpdate");
            return RedirectToAction("Index", "Home");
        }
    }
}
