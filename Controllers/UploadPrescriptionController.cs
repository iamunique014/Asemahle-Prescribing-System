using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class UploadPrescriptionController : Controller
    {
        public IActionResult UploadPrescription()
        {
            return View();
        }
        public IActionResult LoadPrescription()
        {
            return View();
        }
        public IActionResult PrescriptionStatus()
        {
            return View();
        }
        public IActionResult RequestRepeats() {
            return View();

        }

    }
}
