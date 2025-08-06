using Microsoft.AspNetCore.Identity;

namespace TalkRoomDemo.EntityLayer.Concrete
{
        public class AppUser : IdentityUser<int>
        {
            public string Name { get; set; } // Kullanıcının adı
            public string Surname { get; set; } // Kullanıcının adı ve soyadı   
            public string ImageUrl { get; set; } = "/Login/image/pp.jpg"; // Kullanıcının profil resmi URL'si
            public int ConfirmCode { get; set; } // Kullanıcıyı doğrulamak için mail adresinde kullanılan kod
            public string? Bio { get; set; } // Kullanıcının biyografisi
            public string FriendCode { get; set; } // Arkadaşlık isteği göndermek için kullanılan kod
        }
}
