using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.businessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.businessLayer.Concrete
{
    public class FriendsManager : IFriendService
    {
        private readonly IFriendsDal _friendDal;

        public FriendsManager(IFriendsDal friendsDal)
        {
            _friendDal = friendsDal;
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
