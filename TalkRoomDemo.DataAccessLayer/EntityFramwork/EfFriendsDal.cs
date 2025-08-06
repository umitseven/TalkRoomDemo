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
        public async Task <List<AppUserFriendListDto>> GetFriendsByUserId(int userId)
        {

            var context = new Context();
            var values = await context.Friend.Where(f => f.UserId == userId).Select(f => new AppUserFriendListDto
            {
                FriendId = f.Friend.Id,
                FriendUserName = f.Friend.UserName,
                FriendProfilePictureUrl = f.Friend.ImageUrl,
                CurrentUserName = f.User.UserName

            })
                .ToListAsync();
            return values;
        }
    }
}
