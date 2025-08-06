using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.DataAccessLayer.Abstract
{
    public interface IServerUserDal : IGenericDal<ServerUser>
    {
        //Task<List<ServerUserDto>> GetAllServerListAsync(int userId);
    }
}
