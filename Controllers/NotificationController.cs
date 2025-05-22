using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Notification()
        {
            return View();
        }
    }
}
