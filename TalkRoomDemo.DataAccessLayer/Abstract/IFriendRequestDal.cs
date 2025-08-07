using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.DataAccessLayer.Abstract
{
    public interface IFriendRequestDal : IGenericDal<FriendRequest>
    {
        Task<List<AppUserFriendRegisterDto>> GetAllFriendRegister(int userId);
       
    }
}
