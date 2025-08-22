using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.AppDbContext;
using TalkRoomDemo.DataAccessLayer.Repository;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.DataAccessLayer.EntityFramwork
{
    public class EfSereverDal : GenericRepository<Server>, IServerDal
    {
        private readonly Context _context;
        public EfSereverDal(Context context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ServerListDto>> GetAllServerListAsync(int userId)
        {
            var serverIds = await _context.ServerUsers
            .Where(su => su.UserId == userId)
            .Select(su => su.ServerId)
            .ToListAsync();

            // 2. Bu sunucu ID'lerine göre sunucu bilgilerini getir
            var servers = await _context.Servers
                .Include(s => s.CreatorUser)
                .Where(s => serverIds.Contains(s.Id))
                .Select(s => new ServerListDto
                {
                    ServerID = s.Id,
                    ServerName = s.Name,
                    ServerImageUrl = s.ServerImageUrl,
                    CreatorUserId = s.CreatorUserId,
                    CreatorUserName = s.CreatorUser.UserName
                }).ToListAsync();

            return servers;

        }
        //public async Task UpdateAsync(ServerListDto dto)
        //{
        //    var update = await 
        //}


    }
}
