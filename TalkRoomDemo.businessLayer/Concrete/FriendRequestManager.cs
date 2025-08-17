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
       
    }
}
