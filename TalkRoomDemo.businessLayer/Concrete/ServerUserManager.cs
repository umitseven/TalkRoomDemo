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
    public class ServerUserManager : IServerUserService
    {
        private readonly IServerUserDal _serverUserDal;
        public ServerUserManager(IServerUserDal serverUserService)
        {
            _serverUserDal = serverUserService;
        }

        public async Task<List<ServerUserDto>> GetAllServerUserDtoServerIdAsync(int serverId)
        {
            return await _serverUserDal.GetAllServerUserDtoServerIdAsync(serverId);
        }

        public void TDelete(ServerUser entity)
        {
            _serverUserDal.Delete(entity);
        }

        public List<ServerUser> TGetAll()
        {
            return _serverUserDal.GetAll();
        }

        public ServerUser TGetById(int id)
        {
            return _serverUserDal.GetById(id);
        }

        public void TInsert(ServerUser entity)
        {
            _serverUserDal.Insert(entity);
        }

        public void TUpdate(ServerUser entity)
        {
            _serverUserDal.Update(entity);
        }
    }
}
