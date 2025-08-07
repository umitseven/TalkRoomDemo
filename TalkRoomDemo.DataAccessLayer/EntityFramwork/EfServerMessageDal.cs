using Azure;
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
    public class EfServerMessageDal : GenericRepository<ServerMessage>, IServerMessageDal
    {
        private readonly Context _context;
        public EfServerMessageDal(Context context) : base(context)
        {
            _context = context;
        }
        public async Task<List<ServerMessageDto>> GetAllServerMessagesByServerIdAsync(int serverId, int page = 1, int pageSize = 20)
        {
            var skip = (page - 1) * pageSize;
            var value = await _context.ServerMessages
                .Where(sm => sm.ServerId == serverId).OrderByDescending(sm => sm.SendAt)
                .Skip(skip).Take(pageSize).Select(sm => new ServerMessageDto
                {
                    ServerId = sm.ServerId,
                    ServerName = sm.Server.Name,
                    SenderId = sm.SenderUserId,
                    SenderName = sm.SenderUser.UserName,
                    SenderAvatarUrl = sm.SenderUser.ImageUrl,
                    Content = sm.Content,
                    SendAt = sm.SendAt
                }).ToListAsync();
            return value;
        }
    }
}
