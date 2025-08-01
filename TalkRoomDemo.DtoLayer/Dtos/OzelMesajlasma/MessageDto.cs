using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos.CreateMessageDto
{
    public class MessageDto
    {
        public string SenderUserName { get; set; } // Gönderen kullanıcının adı
        public string ReceiverUserName { get; set; } // Alıcı kullanıcının adı
        public string Content { get; set; } // Mesaj içeriği
        public DateTime SendAt { get; set; } // Mesajın gönderildiği tarih ve saat
        public string SenderProfileImageUrl { get; set; } // Gönderen kullanıcının profil fotoğrafı URL'si
        public string ReceiverProfileImageUrl { get; set; } // Gönderen kullanıcının profil fotoğrafı URL'si
        public bool IsRead { get; set; } // Mesajın okunup okunmadığı durumu

    }
}
