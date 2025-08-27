using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Framework.Profiler;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.businessLayer.Concrete;
using TalkRoomDemo.DataAccessLayer.AppDbContext;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.PresentationLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IServerUserService _serverUserService;
        private readonly IFriendService _friendService;
        private readonly OnlineUserCache _onlineCache;
        public static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();
        public ChatHub(IServerUserService serverUserService, OnlineUserCache onlineUserCache,IFriendService friendService)
        {
            _friendService = friendService;
            _serverUserService = serverUserService;
            _onlineCache = onlineUserCache;
        }
      
        public async Task SendMessage(string user, string profileUrl, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, profileUrl, message);
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
    