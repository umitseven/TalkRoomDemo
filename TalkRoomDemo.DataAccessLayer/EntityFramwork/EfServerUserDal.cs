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
using static System.Net.Mime.MediaTypeNames;

namespace TalkRoomDemo.DataAccessLayer.EntityFramwork
{
    public class EfServerUserDal : GenericRepository<ServerUser>, IServerUserDal
    {
        private readonly Context _context;
        public EfServerUserDal(Context context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ServerUserDto>> GetServerUsersAsync(int serverId)
        {
            var values = await _context.ServerUsers
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

        public async Task<List<ServerUserDto>> GetAllServerUserListAsync(int userId)
        {
            var servers = await (from su in _context.ServerUsers
                                 join s in _context.Servers on su.ServerId equals s.Id
                                 join u in _context.Users on s.CreatorUserId equals u.Id
                                 where su.UserId == userId
                                 select new ServerUserDto
                                 {
                                     ServerID = s.Id,
                                     ServerName = s.Name,
                                     ServerImageUrl = s.ServerImageUrl,
                                     CreatorUserId = s.CreatorUserId,
                                     CreatorUserName = u.UserName
                                 }).ToListAsync();
            Console.WriteLine("ServerUser eşleşen kayıt sayısı: " + servers.Count);


            return servers;
        }

    }
}
