using Microsoft.AspNetCore.Mvc;
using PrescribingSystem.Models.ViewModels;

namespace PrescribingSystem.Controllers
{
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult generateReport()
        {
            return View(new reportViewmodel());
        }

        [HttpPost]
        public IActionResult GenerateReport(reportViewmodel model)
        {
            // Placeholder: Add logic to generate PDF based on filters
            // You can use libraries like iTextSharp, DinkToPdf, QuestPDF, etc.
            ViewBag.Message = "PDF report generation is triggered.";
            return View("Index", model);
        }
        public IActionResult GetReport()
        {
            return View();
        }
    }
}
