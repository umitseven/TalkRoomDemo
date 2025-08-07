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
            var value = await _context.Servers.Where(s => s.ServerUsers.Any(su => userId == userId))
                .Select(s => new ServerListDto
                {
                    ServerID = s.Id,
                    ServerName = s.Name,
                    ServerImageUrl = s.ServerImageUrl,
                    CreatorUserId = s.CreatorUserId,
                    CreatorUserName = s.CreatorUser.UserName,

                }).ToListAsync();
            return value;
        }
    }
}
