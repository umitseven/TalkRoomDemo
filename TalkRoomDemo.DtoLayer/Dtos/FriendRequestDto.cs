using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string SenderUserName { get; set; }
        public string SenderProfilePictureUrl { get; set; }
        public DateTime SendAt { get; set; }
    }
}
