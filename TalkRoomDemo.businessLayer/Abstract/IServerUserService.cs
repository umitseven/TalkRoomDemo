using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Abstract
{
    public interface IServerUserService : IGenericService<ServerUser>
    {
        Task<List<ServerUserDto>> GetServerUsersAsync(int serverId);
        Task<List<ServerUserDto>> GetAllServerUserListAsync(int userId);
        void UserConnected(int userId);
        void UserDisconnected(int userId);
        bool IsOnline(int userId);
        List<int> GetOnlineUsers();
    }
}
