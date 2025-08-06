using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class ServerListDto
    {
        public int ServerID { get; set; } // kanalın ID'si
        public string ServerName { get; set; } // kanalın adı
        public List<ServerMessageDto> Messages { get; set; } // kanalın mesajları
        public string ServerImageUrl { get; set; } // kanalın resim URL'si
        public int CreatorUserId { get; set; } // kanalı oluşturan kullanıcının ID'si
        public string CreatorUserName { get; set; } // kanalı oluşturan kullanıcının adı

    }
}
