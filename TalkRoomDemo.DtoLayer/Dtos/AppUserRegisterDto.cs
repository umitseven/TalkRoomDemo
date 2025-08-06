using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkRoomDemo.DtoLayer.Dtos
{
    public class AppUserRegisterDto
    {
        [Required(ErrorMessage = " Ad alanı boş geçilemez")]
        public string Name { get; set; }
        [Required(ErrorMessage = "soyad kısmı boş geçilemez")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "mail adresi boş gelimez")]
        [EmailAddress(ErrorMessage = "geçerli bir e-posta giriniz")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şifre Alanı boş olamaz")]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage = "şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
