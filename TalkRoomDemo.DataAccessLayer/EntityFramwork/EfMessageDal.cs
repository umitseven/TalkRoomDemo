using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.Repository;
using TalkRoomDemo.DataAccessLayer.AppDbContext;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace TalkRoomDemo.DataAccessLayer.EntityFramwork
{
    public class EfMessageDal : GenericRepository<Message>, IMessageDal
    {
        private readonly Context _context;
        public EfMessageDal(Context context) : base(context)
        {
            _context = context;
        }
        public async Task <List<MessageDto>> GetAllMessagesByUserIdAsync(int userId)
        {

            var values = await _context.Messages.Where(m => m.SenderUserId == userId || m.ReceiverUserId == userId).Select(m => new MessageDto
            {
               Id = m.Id,
               SenderUserName = m.SenderUser.UserName,
               Content = m.Content,
               SendAt = m.SendAt,
               ReceiverUserName = m.ReceiverUser.UserName,

            }).ToListAsync();

            return values;
        }
    }
}
