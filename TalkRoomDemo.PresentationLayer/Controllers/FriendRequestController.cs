using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DtoLayer.Dtos;
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
        private readonly INotyfService _notyf;

        public FriendRequestController(IFriendRequestService friendRequestService, UserManager<AppUser> userManager, IFriendService friendService, IHubContext<ChatHub> hubContext, INotyfService notyf)
        {
            _notyf = notyf;
            _friendRequestService = friendRequestService;
            _userManager = userManager;
            _friendService = friendService;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> AcceptInvite(string inviteCode)
        {
            if (string.IsNullOrEmpty(inviteCode))
            { 
                
                _notyf.Warning("Lütfen geçerli bir kod giriniz.");
                return RedirectToAction("Index", "Home");
            }
            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserIdStr))
            { 
                return Unauthorized();
            }
            int currentUserId = int.Parse(currentUserIdStr);

            var targetUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.FriendCode == inviteCode);

            if(targetUser == null)
            {
                _notyf.Warning("Böyle bir davet kodu bulunamadı.");
                return RedirectToAction("Index", "Home");
            }

            if (targetUser.Id == currentUserId)
            {
                _notyf.Warning("Kendine Arkadaşlık isteği gönderemezsin.");
                return RedirectToAction("Index", "Home");
            }

            var existingRequest = await _friendRequestService.GetExistingRequestAsync(currentUserId, targetUser.Id); 

            if(existingRequest != null && existingRequest.IsAccepted == 0)
            {
                _notyf.Warning("Kullanıcıya daha önce davet gönderildi.");
                return RedirectToAction("Index", "Home");
            }
            else if(existingRequest != null && existingRequest.IsAccepted == 1)
            {
                _notyf.Warning("Kullancı ile arkadaşsınız.");
                return RedirectToAction("Index", "Home");
            }

                var friendRequest = new FriendRequest
                {
                    SenderUserId = currentUserId,
                    ReceiverUserId = targetUser.Id,
                    SendAt = DateTime.Now,
                    IsAccepted = 0 // beklemeye alındı
                };
            // 0 bekleme 1 kabul edildi 2 reddedildi
           
            await _friendRequestService.TInsertAsync(friendRequest);
            //await _friendService.CreateFriendshipAsync(currentUserId, targetUser.Id);
            await _hubContext.Clients.User(currentUserId.ToString()).SendAsync("RecaiveFriendRequstUpdate");
            await _hubContext.Clients.User(targetUser.Id.ToString()).SendAsync("RecaiveFriendRequstUpdate");
            await _hubContext.Clients.User(targetUser.Id.ToString()).SendAsync("ReceiveFriendRequest", User.Identity.Name, friendRequest.Id);
            _notyf.Success("Davet gönderildi.");
            return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        public async Task<IActionResult> approvedInvite(int requestId)
        {
            var curentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currenUsertId = int.Parse(curentUserIdStr);

            var request = await _friendRequestService.GetByIdAsync(requestId);

            if (request.ReceiverUserId != currenUsertId)
                return Unauthorized();

            

            var friendRequest = new FriendRequest
            {   
                SenderUserId = currenUsertId,
                ReceiverUserId = request.Id,
                SendAt = DateTime.Now,
                IsAccepted = 0 // beklemeye alındı
            };

            if (request.IsAccepted == 0)
            { 
                await _friendRequestService.TUpdateAsync(request);

                request.IsAccepted = 1; // onaylandı
            
                await _friendService.CreateFriendshipAsync(request.SenderUserId, request.ReceiverUserId);
                await _hubContext.Clients.User(request.SenderUserId.ToString()).SendAsync("ReceiveFriendListUpdate");
                await _hubContext.Clients.User(request.ReceiverUserId.ToString()).SendAsync("ReceiveFriendListUpdate");
                await _hubContext.Clients.User(currenUsertId.ToString()).SendAsync("RecaiveFriendListUpdate"); // buraya signal bağlanacak

                _notyf.Success("Davet kabul edildi.");
                return RedirectToAction("Index","Home");
            }
            
            return RedirectToAction("GetFriendList"); 
        }

        [HttpPost]
        public async Task<IActionResult> RejectInvite(int requestId)
        {
            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUserId = int.Parse(currentUserIdStr);

            var request = await _friendRequestService.GetByIdAsync(requestId);
            if(request.ReceiverUserId != currentUserId)
            {
                return Unauthorized();
            }

            if(request.IsAccepted == 0)
            { 

                request.IsAccepted = 2; // reddedildi
                await _friendRequestService.TUpdateAsync(request);
                _notyf.Warning("Kullanıcı reddedildi");
                return RedirectToAction("GetFriendList","");
            }

            await _hubContext.Clients.User(currentUserId.ToString()).SendAsync("RecaiveFriendListUpdate");
            return BadRequest("Bu istek zaten işlenmiş.");
        }

        [HttpGet]
        public async Task<IActionResult> GetFriendList()
        {
            var userStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userStr)) return PartialView("GetFriendList", null);
            var UserId = int.Parse(userStr); // bu senderId yani kullanıcının kendi Idsi
            var user = await _friendRequestService.GetFriendRequestsByReceiverId(UserId);
            if (user == null)
            {
                Console.WriteLine("FriendRequestDto null döndü.");
                return PartialView("GetFriendList", null);
            }
            await _hubContext.Clients.User(UserId.ToString()).SendAsync("ReceiveFriendListUpdate"); // bu kısım düzgün çalışmıyor burayı düzenle, alıcı ve göndren kişi aynı anda bildirim gidecek
            return PartialView("GetFriendList", user);

        }

    }
}
