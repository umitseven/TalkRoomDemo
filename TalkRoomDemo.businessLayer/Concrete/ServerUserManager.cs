using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Concrete
{
    public class ServerUserManager : IServerUserService
    {
        private readonly IServerUserDal _serverUserDal;
       
        private readonly HashSet<int> _onlineUsers = new HashSet<int>();
        public ServerUserManager(IServerUserDal serverUserService)
        {
            _serverUserDal = serverUserService;
            
        }

        public async Task<List<ServerUserDto>> GetServerUsersAsync(int serverId)
        {
            var users = await _serverUserDal.GetServerUsersAsync(serverId);
            return users.Select(u => new ServerUserDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                AvatarUrl = u.AvatarUrl,
                IsOnline = _onlineUsers.Contains(u.UserId)
            }).ToList();
        }

        public List<int> GetOnlineUsers()
        {
            return _onlineUsers.ToList();
        }

        public bool IsOnline(int userId)
        {
            return _onlineUsers.Contains(userId);
        }

        public void TDelete(ServerUser entity)
        {
            _serverUserDal.Delete(entity);
        }

        public List<ServerUser> TGetAll()
        {
            return _serverUserDal.GetAll();
        }

        public ServerUser TGetById(int id)
        {
            return _serverUserDal.GetById(id);
        }

        public void TInsert(ServerUser entity)
        {
            _serverUserDal.Insert(entity);
        }

        public void TUpdate(ServerUser entity)
        {
            _serverUserDal.Update(entity);
        }
        public void UserConnected(int userId)
        {
            _onlineUsers.Add(userId);
        }

        public void UserDisconnected(int userId)
        {
            _onlineUsers.Remove(userId);    
        }
      
    }
}
