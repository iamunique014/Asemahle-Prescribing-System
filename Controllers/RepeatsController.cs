using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class RepeatsController : Controller
    {
        public IActionResult Repeats()
        {
            return View();
        }
    }
}
