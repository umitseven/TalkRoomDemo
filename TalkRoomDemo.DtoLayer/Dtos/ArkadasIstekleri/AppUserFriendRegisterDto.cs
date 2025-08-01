using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos.FriendRequestDto
{
    public class AppUserFriendRegisterDto
    {
        public int SenderUserId { get; set; } // gönderen kullanıcı ID'si
        public string SenderUserName { get; set; } // gönderen kullanıcı adı
        public int ReceiverUserId { get; set; } // alıcı kullanıcı ID'si
        public string ReceiverUserName { get; set; } // alıcı kullanıcı adı
        public DateTime RequestSentAt { get; set; } // isteğin gönderildiği tarih ve saat
    }
}
