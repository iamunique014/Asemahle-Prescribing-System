using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;

namespace PrescribingSystem.Controllers
{
    //[Authorize(Roles = "Customer")]
    public class PrescriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PrescriptionController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Prescription/Upload
        [HttpGet]
        public IActionResult Upload()
        {
            var viewmodel = new PrescriptionUploadViewModel
            {
                Medications = new List<MedicationLineViewModel>()
            };

            ViewBag.Medications = _context.Medication
                .Where(m => m.Status) // active only
                .Select(m => new SelectListItem
                  {
                     Value = m.MedicationId.ToString(),
                     Text = m.Name
                  })
                .ToList();

            //var medications = _context.Medication
            //   .ToList();

            return View(viewmodel);

        }

        // POST: /Prescription/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(PrescriptionUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Medications = _context.Medication
                    .Where(m => m.Status)
                    .Select(m => new SelectListItem
                    {
                        Value = m.MedicationId.ToString(),
                        Text = m.Name
                    })
                    .ToList();

                return View(model);
            }

            // Validate file
            if (model.PrescriptionFile == null || model.PrescriptionFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a PDF file to upload.");
                return ReloadView(model);
            }

            if (Path.GetExtension(model.PrescriptionFile.FileName).ToLower() != ".pdf")
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                return ReloadView(model);
            }

            if (model.PrescriptionFile.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("", "File size must be less than 10MB.");
                return ReloadView(model);
            }

            // Start transaction for atomicity 
            using var transaction = await _context.Database.BeginTransactionAsync();
            string filePath = string.Empty;

            try
            {
                // Save file inside transaction
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "prescriptions");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PrescriptionFile.FileName)}";
                filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PrescriptionFile.CopyToAsync(stream);
                }

                var prescription = new Prescription
                {
                    CustomerId = "8a43dadf-0a54-4703-b40b-c55784374498", // TODO: Replace with logged-in user later
                    DoctorName = model.DoctorName,
                    DateIssued = DateTime.Now,
                    TotalRepeats = model.TotalRepeats,
                    RemainingRepeats = model.TotalRepeats,
                    TotalCost = model.Medications?.Sum(m => m.Price * m.Quantity) ?? 0,
                    FilePath = $"/uploads/prescriptions/{fileName}",
                    PrescriptionStatus = PrescriptionStatus.Pending
                };

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();

                if (model.Medications == null || !model.Medications.Any())
                    throw new InvalidOperationException("A prescription must contain at least one medication.");

                foreach (var med in model.Medications)
                {
                    var medicationItem = new MedicationItem
                    {
                        PrescriptionId = prescription.PrescriptionId,
                        MedicationId = med.MedicationId,
                        Dosage = med.Dosage,
                        Quantity = med.Quantity,
                        Price = med.Price
                    };
                    _context.MedicationItems.Add(medicationItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                TempData["Success"] = "Prescription uploaded successfully!";
                return RedirectToAction("MyPrescriptions");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                // Delete file if transaction fails
                if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                ModelState.AddModelError("", $"Failed to upload prescription: {ex.Message}");
                return ReloadView(model);
            }
        }

        private IActionResult ReloadView(PrescriptionUploadViewModel model)
        {
            ViewBag.Medications = _context.Medication
                .Where(m => m.Status)
                .Select(m => new SelectListItem
                {
                    Value = m.MedicationId.ToString(),
                    Text = m.Name
                })
                .ToList();

            return View("Upload", model);
        }
        // GET: /Prescription/MyPrescriptions
        public async Task<IActionResult> MyPrescriptions()
        {
            var userId = "8a43dadf-0a54-4703-b40b-c55784374498"; // Or use UserManager to get UserId

            var prescriptions = await _context.Prescriptions
                .Where(p => p.CustomerId == userId)
                .OrderByDescending(p => p.DateIssued)
                .ToListAsync();

            return View(prescriptions);
        }

        public async Task<IActionResult> Download(int id)
        {
            var prescription = await _context.Prescriptions.FindAsync(id);
            if (prescription == null)
                return NotFound();

            var filePath = Path.Combine(_env.WebRootPath, prescription.FilePath.TrimStart('/'));
            var fileName = Path.GetFileName(filePath);

            var mimeType = "application/pdf";
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileBytes, mimeType, fileName);
        }

    }
}
