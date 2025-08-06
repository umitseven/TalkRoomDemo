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
        public async Task<List<AppUserFriendRegisterDto>> GetAllFriendRegister(int userId)
        {
            var context = new Context();
            var values = await context.FriendRequests
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
    }
}
