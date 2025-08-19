using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Abstract
{
    public interface IFriendRequestService : IGenericService<FriendRequest>
    {
        Task<List<AppUserFriendRegisterDto>> TGetAllFriendRegister(int userId);
        Task TInsertAsync(FriendRequest entity);
        Task<FriendRequest> GetByIdAsync(int id);
        Task<List<FriendRequestDto>> GetFriendRequestsByReceiverId(int id);
        Task TUpdateAsync(FriendRequest entity);
        Task<List<FriendRequest>> GetPendingRequestByReceiverAsync(int receiverId);
        Task TDeleteAsync(FriendRequest entity);
    }
}
