using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class ContactUSController : Controller
    {
        public IActionResult ContactUS()
        {
            return View();
        }
    }
}
