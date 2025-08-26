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
    public class ServerManager : IServerService
    {
        private readonly IServerDal _serverDal;

        public ServerManager(IServerDal serverDal)
        {
            _serverDal = serverDal;
        }
        public void TDelete(Server entity)
        {
            _serverDal.Delete(entity);
        }

        public List<Server> TGetAll()
        {
            return _serverDal.GetAll();
        }

        public Server TGetById(int id)
        {
            return _serverDal.GetById(id);
        }
        public async Task<List<ServerListDto>> TGetAllServerListAsync(int userId)
        {
            // :) creator ??
            return await _serverDal.GetAllServerListAsync(userId);

        }

        public void TInsert(Server entity)
        {
            _serverDal.Insert(entity);
        }

        public void TUpdate(Server entity)
        {
            _serverDal.Update(entity);
        }

        public Task<List<ServerUserDto>> TGetAllServerUserListAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Server> GetByIdAsync(int id)
        {
            return await _serverDal.GetByIdAsync(id);
        }
        public async Task UpdateAsync(ServerListDto dto)
        {
            var entity = await _serverDal.GetByIdAsync(dto.ServerID);
            if (entity == null) throw new Exception("Sunucu bulunamadı");

            entity.Name = dto.ServerName;
            entity.ServerImageUrl = dto.ServerImageUrl;

            await _serverDal.UpdateAsync(entity);
        }

        public void TDelete(int id)
        {
            _serverDal.DeleteWithRelations(id);
        }
    }
}
