using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.EntityLayer.Concrete
{
    public class FriendRequest
    {
        public int Id { get; set; } // isteğin ID'si
        public int SenderUserId { get; set; } // gönderen kullanıcı ID'si
        public AppUser SenderUser { get; set; } // gönderen kullanıcın adı

        public int ReceiverUserId { get; set; } // alıcı kullanıcı ID'si
        public AppUser ReceiverUser { get; set; } // alıcı kullanıcının adı

        public DateTime SendAt { get; set; } // isteğin gönderildiği tarih ve saat
        public bool IsAccepted { get; set; } // isteğin kabul edilip edilmediği
    }
}
