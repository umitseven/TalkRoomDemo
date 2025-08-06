using System.ComponentModel.DataAnnotations;

namespace TalkRoomDemo.PresentationLayer.Models
{
    public class SettingsViewModel
    {
        [Url(ErrorMessage = "Geçerli bir URL girin.")]
        public string ImageUrl { get; set; } = "/Login/image/pp.jpg"; // Kullanıcının profil resmi URL'si

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-20 karakter arasında olmalıdır.")]
        [RegularExpression("^[a-zA-Z0-9_]+$", ErrorMessage = "Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.")]
        public string? UserName { get; set; } // Kullanıcının adı

        [StringLength(150, ErrorMessage = "Biyografi en fazla 150 karakter olabilir.")]
        public string? Bio { get; set; } // Kullanıcının biyografisi
    }
}
