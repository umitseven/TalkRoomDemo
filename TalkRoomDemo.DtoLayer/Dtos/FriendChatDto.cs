using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class FriendChatDto
    {
        public int Id { get; set; }
        public int FriendId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FriendName { get; set; }
        public string FriendImageUrl { get; set; }
        public bool IsOnline { get; set; }
    }
}
