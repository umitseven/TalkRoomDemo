using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkRoomDemo.DtoLayer.Dtos;

namespace TalkRoomDemo.DtoLayer.ViewModel
{
    public  class ServerDetailsViewModel
    {
        // Sunucu bilgisi
        public ServerListDto Server { get; set; }

        // Mesaj listesi
        public List<ServerMessageDto> Messages { get; set; }
    }
}
