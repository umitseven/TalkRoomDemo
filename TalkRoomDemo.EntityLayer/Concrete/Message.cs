using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.EntityLayer.Concrete
{
    public class Message
    {
        public int Id { get; set; } // Message ID
        public int SenderUserId { get; set; } // gönderen kullanıcı ID'si
        public int ReceiverUserId { get; set; } // alıcı kullanıcı ID'si
        public string Content { get; set; } // mesaj içeriği
        public DateTime SendAt { get; set; } // mesajın gönderildiği tarih ve saat

        public AppUser SenderUser { get; set; } // gönderen kullanıcı bilgileri
        public AppUser ReceiverUser { get; set; } // alıcı kullanıcı bilgileri
    }
}
