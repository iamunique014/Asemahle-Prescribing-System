using Microsoft.AspNetCore.Mvc;

namespace PrescribingSystem.Controllers
{
    public class PharmacistController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Prescriptions()
        {
            return View();
        }
        public IActionResult Dispense()
        {
            return View();
        }
        public IActionResult DispenseWalkIn()
        {
            return View();
        }
        public IActionResult Doctors()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult LoadOrders()
        {
            return View();
        }
        public IActionResult DoctorDetails()
        {
            return View();
        }
    }
}
