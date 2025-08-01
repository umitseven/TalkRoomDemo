using TalkRoomDemo.DtoLayer.Dtos.RegisterDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Mail;
using TalkRoomDemo.EntityLayer.Concrete;


namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        
        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
        {
            if(ModelState.IsValid)
            {
                Random rnd = new Random();
                int Code;
                Code = rnd.Next(100000, 1000000);

                AppUser appUser = new AppUser()
                {
                    UserName = appUserRegisterDto.UserName,
                    Surname = appUserRegisterDto.Surname,
                    Name = appUserRegisterDto.Name,
                    Email = appUserRegisterDto.Email,
                    ImageUrl ="www.google.com",
                    ConfirmCode =Code
                };
                var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);
                if (result.Succeeded)
                {
                    MimeMessage mimeMessage = new MimeMessage(); // sınıf oluşturma
                    MailboxAddress mailboxAddressFrom = new MailboxAddress("TalkRoom", "xjudhko@gmail.com");
                    MailboxAddress mailboxAddressTo = new MailboxAddress("User", appUser.Email);

                    mimeMessage.From.Add(mailboxAddressFrom);
                    mimeMessage.To.Add(mailboxAddressTo);

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = "Kayıt işlemini tamamlamak için gereken onay kodunuz: " + Code;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    mimeMessage.Subject = "TalkRoom Onay Kodu";

                    MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate("xjudhko@gmail.com", "fofmpvlmrihbbjgk");
                    client.Send(mimeMessage);
                    client.Disconnect(true);

                    TempData["mail"] = appUserRegisterDto.Email;
                    TempData["name"] = appUserRegisterDto.UserName;
                    return RedirectToAction("Index", "ConfirmMail");
                }
                else
                { 
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View();
        }
    }
}
