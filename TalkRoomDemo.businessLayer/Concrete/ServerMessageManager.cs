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
    public class ServerMessageManager : IServerMessageService
    {
        private readonly IServerMessageDal _serverMessageDal;

        public ServerMessageManager(IServerMessageDal serverMessageDal)
        {
            _serverMessageDal = serverMessageDal;
        }

        public async Task<List<ServerMessageDto>> GetAllServerMessagesByServerIdAsync(int serverId, int page = 1, int pageSize = 20)
        {
            return await _serverMessageDal.GetAllServerMessagesByServerIdAsync(serverId, page, pageSize);
        }

        public void TDelete(ServerMessage entity)
        {
            _serverMessageDal.Delete(entity);
        }

        public List<ServerMessage> TGetAll()
        {
            return _serverMessageDal.GetAll();
        }

        public ServerMessage TGetById(int id)
        {
            return _serverMessageDal.GetById(id);
        }

        public void TInsert(ServerMessage entity)
        {
            _serverMessageDal.Insert(entity);
        }

        public async Task TInsertAsync(ServerMessage entity)
        {
            await _serverMessageDal.InsertAsync(entity);
        }

        public void TUpdate(ServerMessage entity)
        {
            _serverMessageDal.Update(entity);
        }
    }
}
