using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Abstract
{
    public interface IMessageService :IGenericService<Message>
    {
       Task<List<MessageDto>> TGeAlltMessagesByUserId(int userId);
        Task TInsertAsync(Message entity);
    }
}
