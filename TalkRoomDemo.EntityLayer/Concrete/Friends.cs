using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.EntityLayer.Concrete
{
    public class Friends
    {
        public int Id { get; set; } 

        public int UserId { get; set; } // Arkadaşlık başlatan kullanıcı isteği gönderen kişi
        public AppUser User { get; set; } 

        public int FriendId { get; set; } // Arkadaş olunan kişi 
        public AppUser Friend { get; set; } //app user tablosundan gelen arkadaş olunan kişi

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
