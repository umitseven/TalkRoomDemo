using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.DtoLayer.ViewModel;
using TalkRoomDemo.EntityLayer.Concrete;
using TalkRoomDemo.PresentationLayer.Hubs;

namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class AddRoomController : Controller
    {
        private readonly IServerService _serverService;
        private readonly IServerUserService _userService;
        private readonly IServerMessageService _serverMessageService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotyfService _notyf;
        private readonly IFriendService _friendService;
        public AddRoomController(IServerService serverService, IHubContext<ChatHub> hubContext, INotyfService notyfService, UserManager<AppUser> userManager, IServerUserService userService, IFriendService friendService, IServerMessageService serverMessageService)
        {
            _serverMessageService = serverMessageService;
            _serverService = serverService;
            _userManager = userManager;
            _hubContext = hubContext;
            _notyf = notyfService;
            _userService = userService;
            _friendService = friendService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null) return PartialView("Index",null);
            int userId = int.Parse(userIdStr); // giriş yapan kullanıcının Id'si

            var user = await _serverService.TGetAllServerListAsync(userId); // Idsine göre bütün kanalları getirecek 

            return PartialView("Index", user);
        }

        [HttpPost]
        public async Task<IActionResult>RoomCreate(ServerListDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int UserId = int.Parse(userIdStr);
            Random random = new Random();
            int code = random.Next(100000, 999999);
            string ServerRandomCode = $"#{code}";

            var server = new Server
            {
                Name = dto.ServerName,
                CreatorUserId = UserId,
                ServerImageUrl = dto.ServerImageUrl ?? "/Login/image/RoomIcon.svg",
                ServerCode = ServerRandomCode
            };
            _serverService.TInsert(server);

            var serverUser = new ServerUser
            {
                UserId = UserId, 
                ServerId = server.Id,
                JoinedAt = DateTime.Now,
                Role = "Owner"
            };
            _userService.TInsert(serverUser);
            
            _notyf.Success("Yeni oda Başarılı bir şekilde kuruldu.");
            return RedirectToAction("Details", new { id = server.Id });
        }
        [HttpPost]
        public async Task<IActionResult> GetServerUser(string userName, int serverId)
        {
            var user =await _userManager.FindByNameAsync(userName);
            var serveruser = new ServerUser
            {
                UserId = user.Id,
                ServerId = serverId,
                Role = "Owner",
            };
            _userService.TInsert(serveruser);
            _notyf.Success($"{user.UserName} Odaya başarılı bir şekilde eklendi.");
            return RedirectToAction("Details", new { id = serverId });
        }

        [HttpGet]
        public async Task<IActionResult> GetServerUsers(int serverId)
        {
            try
            {
                var users = await _userService.GetAllServerUserListAsync(serverId);
                return PartialView("GetServerUsers", users);
            }
            catch (Exception ex)
            {
                return Content("Hata: " + ex.Message);
            }
        }
        public async Task<IActionResult> Details(int id)
        {
            
            var server = await _serverService.GetByIdAsync(id);
            if(server == null) return NotFound();

            var serverMessages = await _serverMessageService.GetAllServerMessagesByServerIdAsync(id);

            var viewModel = new ServerDetailsViewModel
            {
                Server = new ServerListDto
                {
                    ServerID = server.Id,
                    ServerName = server.Name,
                    ServerImageUrl = server.ServerImageUrl
                },
                Messages = serverMessages
            };

            // Session’a sunucu adını kaydet
            HttpContext.Session.SetString("SelectedRoom", server.Name);

            // View’e ViewModel gönder
            return View(viewModel);
        }
      
        
        [HttpGet]
        public async Task<IActionResult> Settings(int id)
        {
            var channel = await _serverService.GetByIdAsync(id);
            if (channel == null) return NotFound();

            var SettingDto = new ServerListDto
            {
                ServerID = channel.Id,
                ServerName = channel.Name,
                ServerImageUrl = channel.ServerImageUrl,

            };
            ViewBag.CurrentRoomId = id;
            return View(SettingDto);

        }
        [HttpPost]
        public async Task<IActionResult> Settings(ServerListDto dto)
        {
            var channel = await _serverService.GetByIdAsync(dto.ServerID);
            if (channel == null) return NotFound();
            var data = channel.Name;
            await _serverService.UpdateAsync(dto);
            _notyf.Success($"{data} Oda başarılı bir şekilde güncellendi. ");
            return RedirectToAction("Details", new { id = dto.ServerID });
        }
        public async Task <IActionResult> frUser(int id)
        {
            var friend = await _friendService.TGetFriendChatByUserId(id);
            await _hubContext.Clients.All.SendAsync("ReceiveFriendData", friend);
            return View(friend);
        }
        [HttpPost]
        public IActionResult DeleteRoom(int id)
        {
            _serverService.TDelete(id);
            return RedirectToAction("Index", "Home");
        }

    }
}
