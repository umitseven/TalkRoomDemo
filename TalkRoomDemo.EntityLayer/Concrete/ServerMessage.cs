using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.EntityLayer.Concrete
{
    public class ServerMessage
    {
        public int Id { get; set; } // Mesajın benzersiz ID'si

        public int ServerId { get; set; } // Hangi sunucuya ait olduğunu belirtir
        public Server Server { get; set; } // Mesajın ait olduğu sunucu bilgileri

        public int SenderUserId { get; set; } // Mesajı gönderen kullanıcının ID'si
        public AppUser SenderUser  { get; set; } // Mesajı gönderen kullanıcının bilgileri

        public string Content { get; set; } // Mesajın içeriği
        public DateTime SendAt { get; set; } // Mesajın gönderildiği tarih
    }
}
