using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.EntityLayer.Concrete
{
    public class Server
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Sunucuyu oluşturan kullanıcı
        public int CreatorUserId { get; set; } // Oluşturan kullanıcının ID'si
        public AppUser CreatorUser { get; set; } // Oluşturan kullanıcının bilgileri
        public string ServerImageUrl { get; set; } // Sunucuya ait resim URL'si
        public string ServerCode { get; set; } // Sunucuya ait kod (davet kodu vb.)

        // Sunucuya ait mesajlar
        public ICollection<ServerMessage> ServerMessages { get; set; } = new List<ServerMessage>(); // Sunucu mesajları

        // Sunucu üyeleri
        public ICollection<ServerUser> ServerUsers { get; set; } = new List<ServerUser>(); // Sunucu üyeleri
    }
}
