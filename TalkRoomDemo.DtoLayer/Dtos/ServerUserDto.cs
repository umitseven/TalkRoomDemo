using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class ServerUserDto
    {
        public int UserId { get; set; } // kullanıcı ID'leri
        public string UserName { get; set; } // kullanıcı adları
        public string AvatarUrl { get; set; } // kullanıcı avatar URL'leri
        public bool IsOnline { get; set; } // kullanıcının çevrimiçi durumu
    }
}
