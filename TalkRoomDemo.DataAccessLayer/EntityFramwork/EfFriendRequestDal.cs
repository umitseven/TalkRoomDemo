using Microsoft.EntityFrameworkCore;
using TalkRoomDemo.DataAccessLayer.Abstract;
using TalkRoomDemo.DataAccessLayer.AppDbContext;
using TalkRoomDemo.DataAccessLayer.Repository;
using TalkRoomDemo.DtoLayer.Dtos;
using TalkRoomDemo.EntityLayer.Concrete;

namespace TalkRoomDemo.DataAccessLayer.EntityFramwork
{
    public class EfFriendRequestDal : GenericRepository<FriendRequest>, IFriendRequestDal
    {
        private readonly Context _context;
        public EfFriendRequestDal(Context contex) : base(contex)
        {
            _context = contex;
        }
        public async Task<List<AppUserFriendRegisterDto>> GetAllFriendRegister(int userId)
        {
            
            var values = await _context.FriendRequests
                .Where(fr => fr.SenderUserId == userId || fr.ReceiverUserId == userId).Select(fr => new AppUserFriendRegisterDto
                {
                    SenderUserId = fr.SenderUserId,
                    SenderUserName = fr.SenderUser.UserName,
                    ReceiverUserId = fr.ReceiverUserId,
                    ReceiverUserName = fr.ReceiverUser.UserName,
                    RequestSentAt = fr.SendAt,
                    IsAccepted = fr.IsAccepted
                }).ToListAsync();
            return values;
        }
        public async Task<AppUserFriendRegisterDto> GetFriendRequestAsync(int senderId, int receiverId)
        {
            var fr = await _context.FriendRequests
         .Include(fr => fr.SenderUser)
         .Include(fr => fr.ReceiverUser)
         .FirstOrDefaultAsync(fr => fr.SenderUserId == senderId && fr.ReceiverUserId == receiverId);

            if (fr == null) return null;

            return new AppUserFriendRegisterDto
            {
                SenderUserId = fr.SenderUserId,
                SenderUserName = fr.SenderUser.UserName,
                ReceiverUserId = fr.ReceiverUserId,
                ReceiverUserName = fr.ReceiverUser.UserName,
                RequestSentAt = fr.SendAt,
                IsAccepted = fr.IsAccepted
            };

        }
       
    }
}
