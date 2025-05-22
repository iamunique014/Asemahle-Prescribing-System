using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class ManageAllergiesController : Controller
    {
        public IActionResult ManageAllergiess()
        {
            return View();
        }
    }
}
