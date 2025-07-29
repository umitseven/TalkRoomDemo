using Microsoft.AspNetCore.Mvc;

namespace TalkRoomDemo.PresentationLayer.Controllers
{
    public class HomeController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }
    }
}
