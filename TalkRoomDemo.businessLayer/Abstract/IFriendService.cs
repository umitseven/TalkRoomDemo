using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;


namespace TalkRoomDemo.businessLayer.Abstract
{
    public interface IFriendService : IGenericService<Friends>
    {
        Task <List<AppUserFriendListDto>> TGetFriendsByUserId(int id);
        Task CreateFriendshipAsync(int senderUserId, int receiverUserId);
        Task<Friends?> GetExistingRequestAsync(int userId1, int userId2);

    }
}
