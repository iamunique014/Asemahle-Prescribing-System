using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Registration() {

            return View();
        }

    }
}
