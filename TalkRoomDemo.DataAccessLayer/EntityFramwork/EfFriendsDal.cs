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
    public class EfFriendsDal : GenericRepository<Friends>, IFriendsDal
    {
        private readonly Context _context;
        public EfFriendsDal(Context contex) : base(contex)
        {
            _context = contex;
        }

       

        public async Task <List<AppUserFriendListDto>> GetFriendsByUserId(int userId)
        {
            var values = await _context.Friend.Where(f => f.UserId == userId).Select(f => new AppUserFriendListDto
            {
                FriendId = f.Friend.Id,
                FriendUserName = f.Friend.UserName,
                FriendProfilePictureUrl = f.Friend.ImageUrl,
                CurrentUserName = f.User.UserName,
              
            })
                .ToListAsync();
            return values;
        }
        public async Task<FriendChatDto> GetFriendChatByUserId(int id)
        {
            var values = await _context.Friend.Where(f => f.UserId == id).Select(f => new FriendChatDto
            {
                FriendId = f.User.Id,
                FriendName = f.User.UserName,
                FriendImageUrl = f.User.ImageUrl
            }).FirstOrDefaultAsync();

            return values;
        }
    }
}
