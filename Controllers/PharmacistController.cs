using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class PharmacistController : Controller
    {
        public IActionResult PharmacistHome()
        {
            return View();
        }
    }
}
