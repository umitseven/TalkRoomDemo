using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Concrete
{
    public class FriendsManager : IFriendService
    {
        private readonly IFriendsDal _friendDal;
        private readonly OnlineUserCache _onlineUserCache;

        public FriendsManager(IFriendsDal friendsDal, OnlineUserCache onlineUserCache)
        {
            _friendDal = friendsDal;
            _onlineUserCache = onlineUserCache;
        }

        public async Task CreateFriendshipAsync(int senderUserId, int receiverUserId)
        {
            var friendship = new Friends
            {
                UserId = senderUserId,
                FriendId = receiverUserId
            };

            var reverseFriendship = new Friends
            {
                UserId = receiverUserId,
                FriendId = senderUserId
            };
            await _friendDal.InsertAsync(friendship);
            await _friendDal.InsertAsync(reverseFriendship);
        }

        public void TDelete(Friends entity)
        {
            _friendDal.Delete(entity);
        }

        public List<Friends> TGetAll()
        {
           return _friendDal.GetAll();
        }

        public Friends TGetById(int id)
        {
            return _friendDal.GetById(id);
        }

        public async Task <List<AppUserFriendListDto>> TGetFriendsByUserId(int userId)
        {
            return await _friendDal.GetFriendsByUserId(userId);
        }
      
       
        public void TInsert(Friends entity)
        {
            _friendDal.Insert(entity);
        }

        public void TUpdate(Friends entity)
        {
            _friendDal.Update(entity);
        }
    }
}
