using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult landing()
        {
            return View();
        }
    }
}
