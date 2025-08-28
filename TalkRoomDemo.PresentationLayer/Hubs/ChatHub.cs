using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.businessLayer.Concrete;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.PresentationLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServerUserService _serverUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMessageService _messageService;
        private readonly IServerMessageService _serverMessageService;
        private readonly IFriendService _friendService;
        private readonly OnlineUserCache _onlineCache;
        public static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();
        public ChatHub(IServerUserService serverUserService, OnlineUserCache onlineUserCache,IFriendService friendService, IServerMessageService serverMessageService, UserManager<AppUser> userManager, IMessageService messageService)
        {
            _messageService = messageService;
            _userManager = userManager;
            _serverMessageService = serverMessageService;
            _friendService = friendService;
            _serverUserService = serverUserService;
            _onlineCache = onlineUserCache;
        }

        public async Task SendFriendMessage(string UserIdReceiver, string message)
        {
            var user = Context.UserIdentifier;
            int userId = int.Parse(user);
            if (OnlineUsers.TryGetValue(UserIdReceiver, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("UserRecaiverMessage", user, message);
            }

            var FriendMessage = new Message
            {
                SenderUserId = userId,
                ReceiverUserId = int.Parse(UserIdReceiver),
                Content = message,
                SendAt = DateTime.Now
            };
            await _messageService.TInsertAsync(FriendMessage);

        }
        public async Task SendMessage(int roomId ,string user, string profileUrl, string message)
        {
            
                var AppUser = await _userManager.FindByNameAsync(user);
                if (AppUser == null) throw new Exception("Kullanıcı bulunamadı: " + user);

                var serverMessage = new ServerMessage
                {
                    ServerId = roomId,
                    SenderUserId = AppUser.Id,
                    Content = message,
                    SendAt = DateTime.Now,
                };

                await _serverMessageService.TInsertAsync(serverMessage);

            // Grup yerine tüm kullanıcılara gönder
            await Clients.Group(roomId.ToString())
     .SendAsync("ReceiveMessage", user, profileUrl, message);


        }
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
         
        public async Task SendFriendData(object friendData)
        {
            await Clients.All.SendAsync("ReceiveFriendData", friendData);

        }
        

        public override async Task OnConnectedAsync()
        {
            int userId = int.Parse(Context.UserIdentifier);
            var User = Context.UserIdentifier;
            _onlineCache.UserConneted(userId);

            var friends = await _friendService.TGetFriendsByUserId(userId);

            if (!string.IsNullOrEmpty(User))
            {
                OnlineUsers[User] = Context.ConnectionId;
                await Clients.All.SendAsync("UserOnline",User);
            }
           

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int userId = int.Parse(Context.UserIdentifier);
            var user = Context.UserIdentifier;
            await Task.Delay(TimeSpan.FromMinutes(3));

            if (!string.IsNullOrEmpty(user))
            {
                OnlineUsers.TryRemove(user, out _);
                await Clients.All.SendAsync("UserOffline", user);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetOnlineUsers()
        {
            await Clients.Caller.SendAsync("ReceiveOnlineUsers", OnlineUsers.Keys);
        }

        public async Task SendFriendRequest(int requestId, string senderName, string receiverUserId)
        {
            if(OnlineUsers.TryGetValue(receiverUserId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveFriendRequest", senderName, requestId);
            }
        }
        public async Task RespondFriendRequest(string senderUserId, string receiverUserId, int IsAccepted)
        {
            if(OnlineUsers.TryGetValue(senderUserId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("FriendRequestResponse", receiverUserId, IsAccepted);
            }
        }
        public async Task SendFriendListUpdate(string userId)
        {
            await Clients.User(userId).SendAsync("RecaiveFriendListUpdate");
        }
       
    }
}
    