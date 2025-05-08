using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult CustomerHome()
        {
            return View();
        }
    }
}
