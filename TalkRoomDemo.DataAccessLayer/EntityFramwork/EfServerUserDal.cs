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
    public class EfServerUserDal : GenericRepository<ServerUser>, IServerUserDal
    {
        public async Task<List<ServerUserDto>> GetAllServerUserDtoServerIdAsync(int serverId)
        {
            var context = new Context();
            var values = await context.ServerUsers
                .Where(su => su.UserId == serverId)
                .Include(su => su.AppUser)
                .Select(su => new ServerUserDto
                {
                    UserId = su.AppUser.Id,
                    UserName = su.AppUser.UserName,
                    AvatarUrl = su.AppUser.ImageUrl
                })
                .ToListAsync();
            return values;
        }
    }
}
