using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class AppUserFriendRegisterDto
    {
        public int SenderUserId { get; set; } // gönderen kullanıcı ID'si
        public string ProfilImage { get; set; } // gönderen kullanıcın profil resmi
        public string SenderUserName { get; set; } // gönderen kullanıcı adı
        public int ReceiverUserId { get; set; } // alıcı kullanıcı ID'si
        public string ReceiverUserName { get; set; } // alıcı kullanıcı adı
        public DateTime RequestSentAt { get; set; } // isteğin gönderildiği tarih ve saat
        public int IsAccepted { get; set; } // isteğin kabul edilip edilmediği durumu
    }
}
