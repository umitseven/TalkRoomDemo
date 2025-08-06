using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using TalkRoomDemo.EntityLayer.Concrete;
using TalkRoomDemo.PresentationLayer.Models;

namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {   
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task <IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User); // Giriş yapmış kullanıcıyı al
            var model = new SettingsViewModel
            {
                ImageUrl = user.ImageUrl, // Kullanıcının profil resmi URL'si
                UserName = user.UserName, // Kullanıcının adı
                Bio = user.Bio // Kullanıcının biyografisi (ad ve soyad)
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel settingsViewModel)
        {
            if (!ModelState.IsValid) 
            {
                return View(settingsViewModel); // Model geçerli değilse, formu tekrar göster

            }
            var user = await _userManager.GetUserAsync(User); // Giriş yapmış kullanıcıyı al
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Kullanıcı bulunamazsa giriş sayfasına yönlendir
            }
            if(!IsValidImageUrl(settingsViewModel.ImageUrl)) 
            {
                user.ImageUrl = "/Login/image/pp.jpg"; // Geçersiz URL ise varsayılan profil resmi URL'sini kullan
            }
            else
            {
                user.ImageUrl = settingsViewModel.ImageUrl; // Kullanıcının profil resmi URL'sini güncelle
            }

            
            user.UserName = settingsViewModel.UserName; // Kullanıcının adını güncelle
            user.Bio = settingsViewModel.Bio; // Kullanıcının biyografisini güncelle

            var result = await _userManager.UpdateAsync(user); // Kullanıcıyı güncelle
           

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserName", user.UserName ?? user.Name),
                    new Claim("ImageUrl", user.ImageUrl ?? "/Login/image/pp.jpg"),
                    new Claim("FriendCodes", user.FriendCode ?? "")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignOutAsync(); // sadece mevcut çerezi siler
                await HttpContext.SignInAsync("Identity.Application", principal); // yeni çerez
                // yeni çerez oluşturur

                TempData["Success"] = "Profil başarıyla güncellendi.";
                return RedirectToAction("Settings"); // Güncelleme başarılıysa ayarlar sayfasına yönlendir
            }
            else 
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); // Hata mesajlarını model durumuna ekle
                }
                return View(settingsViewModel);
            }
        }
        private bool IsValidImageUrl(string url)
        {
            try
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var parsed = new Uri(url);
                var ext = Path.GetExtension(parsed.AbsolutePath).ToLower();

                return (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps) &&
                       validExtensions.Contains(ext);
            }
            catch
            {
                return false;
            }
        }
        
    }
}
