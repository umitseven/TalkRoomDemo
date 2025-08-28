using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class ServerMessageDto
    {
        public int ServerId { get; set; } // server'in ID'si
        public string ServerName { get; set; } // server'in adı

        public int SenderId { get; set; } // mesajı gönderen kullanıcının ID'si
        public string SenderName { get; set; } // mesajı gönderen kullanıcının adı
        public string SenderAvatarUrl { get; set; } // mesajı gönderen kullanıcının avatar URL'si   

        public string Content { get; set; } // mesaj içeriği
        public DateTime SendAt { get; set; } // mesajın gönderildiği zaman
        public string SendAtString => SendAt.ToString("yyyy-MM-ddTHH:mm:ss"); // mesajın gönderildiği zaman (string formatında)




    }
}
