using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.EntityLayer.Concrete;
using TalkRoomDemo.PresentationLayer.Hubs;
using TalkRoomDemo.DtoLayer.Dtos;

namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class AddRoomController : Controller
    {
        private readonly IServerService _serverService;
        private readonly IServerUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotyfService _notyf;
        public AddRoomController(IServerService serverService, IHubContext<ChatHub> hubContext, INotyfService notyfService, UserManager<AppUser> userManager, IServerUserService userService)
        {
            _serverService = serverService;
            _userManager = userManager;
            _hubContext = hubContext;
            _notyf = notyfService;
            _userService = userService;
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
            
            var server = new Server
            {
                Name = dto.ServerName,
                CreatorUserId = UserId,
                ServerImageUrl = dto.ServerImageUrl ?? "/Login/image/RoomIcon.svg"
            };
            _serverService.TInsert(server);

            var serverUser = new ServerUser
            {
                UserId = UserId, //giren kullanı
                ServerId = server.Id,
                JoinedAt = DateTime.Now,
                Role = "Owner"
            };
            _userService.TInsert(serverUser);

            _notyf.Success("Yeni oda Başarılı bir şekilde kuruldu.");
            return RedirectToAction("Index","Home");
        }
    }
}
