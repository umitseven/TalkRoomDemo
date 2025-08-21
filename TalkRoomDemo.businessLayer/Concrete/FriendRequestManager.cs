using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.EntityFramwork;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Concrete
{
    public class FriendRequestManager : IFriendRequestService
    {
        private readonly IFriendRequestDal _friendRequestDal;

        public FriendRequestManager(IFriendRequestDal friendRequestDal)
        {
            _friendRequestDal = friendRequestDal;
        }
        public void TDelete(FriendRequest entity)
        {
            _friendRequestDal.Delete(entity);
        }

        public List<FriendRequest> TGetAll()
        {
            return _friendRequestDal.GetAll();
        }

        public FriendRequest TGetById(int id)
        {
            return _friendRequestDal.GetById(id);
        }
        public async Task<List<AppUserFriendRegisterDto>> TGetAllFriendRegister(int userId)
        {
            return await _friendRequestDal.GetAllFriendRegister(userId);
        }

        public void TInsert(FriendRequest entity)
        {
            _friendRequestDal.Insert(entity);
        }
        public async Task TInsertAsync(FriendRequest entity)
        {
            await _friendRequestDal.InsertAsync(entity); 
        }

        public void TUpdate(FriendRequest entity)
        {
            _friendRequestDal.Update(entity);
        }

        public async Task<FriendRequest> GetByIdAsync(int id)
        {
            var user = await _friendRequestDal.GetByIdAsync(id);
            return user;
        }
        public async Task TUpdateAsync(FriendRequest entity)
        {
            await _friendRequestDal.UpdateAsync(entity);
        }

        public async Task<List<FriendRequest>> GetPendingRequestByReceiverAsync(int receiverId)
        {
            return await _friendRequestDal.GetPendingRequestByReceiverAsync(receiverId);
        }

        public async Task<List<FriendRequestDto>> GetFriendRequestsByReceiverId(int receviverId)
        {
            var requests = await _friendRequestDal.GetRequestsByReceiverId(receviverId);
            var dtoList = new List<FriendRequestDto>();
            foreach (var request in requests)
            {
                if (request.SenderUser == null) continue;

                dtoList.Add(new FriendRequestDto
                {
                    Id = request.Id,
                    SenderUserId = request.SenderUserId,
                    SenderUserName = request.SenderUser.UserName,
                    SenderProfilePictureUrl = request.SenderUser.ImageUrl,
                    SendAt = request.SendAt
                });

            }
            return dtoList;

        }

        public async Task TDeleteAsync(FriendRequest entity)
        {
            await _friendRequestDal.DeleteAsync(entity);
        }

        public async Task<FriendRequest?> GetExistingRequestAsync(int userId1, int userId2)
        {
            return await _friendRequestDal.GetAsync(fr => (fr.SenderUserId == userId1 && fr.ReceiverUserId == userId2) ||
            (fr.SenderUserId == userId2 && fr.ReceiverUserId == userId1)
            );

        }
    }
}
