using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class AppUserFriendListDto
    {
        public int FriendId { get; set; } //listedki arkadaşın Id'si
        public string FriendUserName { get; set; } // listedeki arkadaşın Kullanıcı adı
        public bool IsOnline { get; set; } // listedeki arkadaşın çevrim durumu
        public string FriendProfilePictureUrl { get; set; } // listedeki arkadaşın profil fotoğrafı URL'si
        public bool HasUnreadMessages { get; set; } // listedeki arkadaşın okunmamış mesajları olup olmadığı durumu
        public DateTime? LastMessageTime { get; set; } // listedeki arkadaşla yapılan son mesajın zamanı
        public string CurrentUserName { get; set; } // şu anki kullanıcının kullanıcı adı
    }
}

// yukarıdaki 3 sınıfı kullanmıyoruz message sınıfına taşınacak