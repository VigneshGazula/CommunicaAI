using Microsoft.AspNetCore.Mvc;

namespace CommunicaAI.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
