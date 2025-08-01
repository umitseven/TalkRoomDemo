using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.EntityLayer.Concrete
{
    public class ServerUser
    {
        public int ServerId { get; set; } // Bağlı olduğu sunucunun ID'si
        public Server Server { get; set; } // Bağlı olduğu sunucu bilgileri

        public int UserId { get; set; } // Katılan kullanıcının ID'si
        public AppUser AppUser { get; set; } // Katılan kullanıcının bilgileri

        public DateTime JoinedAt { get; set; } // Kullanıcının sunucuya katıldığı tarih
        public string Role { get; set; }
    }
}
