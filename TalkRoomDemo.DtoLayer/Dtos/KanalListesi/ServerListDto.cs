using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos.KanalIciMesajlasma;

namespace TalkRoomDemo.DtoLayer.Dtos.KanalListesi
{
    public class ServerListDto
    {
        public int ServerID { get; set; } // kanalın ID'si
        public string ServerName { get; set; } // kanalın adı
        public List<ServerMessageDto> Messages { get; set; } // kanalın mesajları
    }
}
