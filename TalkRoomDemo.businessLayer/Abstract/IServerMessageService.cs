using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Abstract
{
    public interface IServerMessageService : IGenericService<ServerMessage>
    {
        Task<List<ServerMessageDto>> GetAllServerMessagesByServerIdAsync(int serverId, int page = 1, int pageSize = 20);
    }
}
