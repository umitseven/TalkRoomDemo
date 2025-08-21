using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Abstract
{
    public interface IServerService : IGenericService<Server>
    {
        Task<List<ServerListDto>> TGetAllServerListAsync(int userId);
        Task<List<ServerUserDto>> TGetAllServerUserListAsync(int userId);

    }
}
