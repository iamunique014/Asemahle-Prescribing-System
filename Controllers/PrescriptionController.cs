using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Path = System.IO.Path;

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
            return View();
        }

        //POST: /Prescription/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(PrescriptionUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate file
            if (model.PrescriptionFile == null || model.PrescriptionFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a PDF file to upload.");
                return View(model);
            }

            if (Path.GetExtension(model.PrescriptionFile.FileName).ToLower() != ".pdf")
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                return View(model);
            }

            if (model.PrescriptionFile.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("", "File size must be less than 10MB.");
                return View(model);
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

                string rawText = "";
                using (PdfReader reader = new PdfReader(filePath))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        rawText += PdfTextExtractor.GetTextFromPage(reader, i);
                    }
                }

                var prescription = new Prescription
                {
                    CustomerId = "8a43dadf-0a54-4703-b40b-c55784374498",
                    DoctorName = null,
                    PrescriptionDate = DateTime.UtcNow,
                    TotalCost = 0,
                    FilePath = $"/uploads/prescriptions/{fileName}",
                    PrescriptionStatus = PrescriptionStatus.Pending,
                    RawText = rawText
                };

                _context.Prescriptions.Add(prescription);
                await _context.SaveChangesAsync();


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

        // GET: /Prescription/MyPrescriptions
        public async Task<IActionResult> MyPrescriptions()
            {
            var userId = "8a43dadf-0a54-4703-b40b-c55784374498"; // Or use UserManager to get UserId

            var prescriptions = await _context.Prescriptions
                .Where(p => p.CustomerId == userId)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();

            return View(prescriptions);
        }

        // GET: /Prescription/PrescriptionDetails
        [HttpGet]
        public IActionResult PrescriptionDetails(int prescriptionId)
        {
            var userId = "8a43dadf-0a54-4703-b40b-c55784374498"; // Or use UserManager to get UserId

            var prescription = _context.Prescriptions
                .Include(p => p.MedicationItems)
                .ThenInclude(mi => mi.Medication)
                .FirstOrDefault(p => p.CustomerId == userId && p.PrescriptionId == prescriptionId);

            return View(prescription);
        }

        public IActionResult DispenseRequest(int prescriptionId)
        {
            var prescriptionOrder = new PrescriptionOrders
            {
                CustomerId = "8a43dadf-0a54-4703-b40b-c55784374498", // Or use UserManager to get UserId
                PrescriptionId = prescriptionId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                IsDeleted = IsDeleted.Active
            };
                
            _context.PrescriptionOrders.Add(prescriptionOrder);

            _context.SaveChanges();

            return RedirectToAction("MyOrders");
        }

        public IActionResult MyOrders()
        {
            string customerId = "8a43dadf-0a54-4703-b40b-c55784374498";

            var orders = _context.PrescriptionOrders;

            return View(orders);
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
       

        public async Task<IActionResult> Download(int prescriptionId)
        {
            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
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























// GET: /Prescription/Upload
//[HttpGet]
//public IActionResult GetPrescription()
//{
//    int prescriptionId = 1014;
//    string filePath;
//    var prescription = _context.Prescriptions
//        .Include(p => p.MedicationItems)
//        .ThenInclude(mi => mi.Medication)
//        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);


//    if (prescription == null) { return NotFound(); }

//    // Save file inside transaction
//    var prescriptionFolder = Path.Combine(_env.WebRootPath, "uploads", "Dummy Prescriptions");
//    if (!Directory.Exists(prescriptionFolder))
//        Directory.CreateDirectory(prescriptionFolder);

//    var fileName = $"{Guid.NewGuid()}{"pdf"}";
//    filePath = Path.Combine(prescriptionFolder, fileName);

//    Document document = new Document();
//    PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
//    document.Open();

//    // Add prescription details to PDF
//    document.Add(new Paragraph("Prescription: 1014"));
//    document.Add(new Paragraph($"Patient ID: {patientId}"));
//    document.Add(new Paragraph($"Date: {date}"));
//    document.Add(new Paragraph($"Doctor: {doctor}"));
//    document.Add(new Paragraph("Medications:"));

//    return View();
//}


//// GET: /Prescription/Upload
//[HttpGet]
//public IActionResult Upload()
//{


//    var viewmodel = new PrescriptionUploadViewModel
//    {
//        Medications = new List<MedicationLineViewModel>()
//    };

//    ViewBag.Medications = _context.Medication
//        .Where(m => m.Status) // active only
//        .Select(m => new SelectListItem
//        {
//            Value = m.MedicationId.ToString(),
//            Text = m.Name
//        })
//        .ToList();

//    //var medications = _context.Medication
//    //   .ToList();

//    return View(viewmodel);

//}

//// POST: /Prescription/Upload
//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Upload(PrescriptionUploadViewModel model)
//{
//    if (!ModelState.IsValid)
//    {
//        ViewBag.Medications = _context.Medication
//            .Where(m => m.Status)
//            .Select(m => new SelectListItem
//            {
//                Value = m.MedicationId.ToString(),
//                Text = m.Name
//            })
//            .ToList();

//        return View(model);
//    }

//    // Validate file
//    if (model.PrescriptionFile == null || model.PrescriptionFile.Length == 0)
//    {
//        ModelState.AddModelError("", "Please select a PDF file to upload.");
//        return ReloadView(model);
//    }

//    if (Path.GetExtension(model.PrescriptionFile.FileName).ToLower() != ".pdf")
//    {
//        ModelState.AddModelError("", "Only PDF files are allowed.");
//        return ReloadView(model);
//    }

//    if (model.PrescriptionFile.Length > 10 * 1024 * 1024)
//    {
//        ModelState.AddModelError("", "File size must be less than 10MB.");
//        return ReloadView(model);
//    }

//    // Start transaction for atomicity 
//    using var transaction = await _context.Database.BeginTransactionAsync();
//    string filePath = string.Empty;

//    try
//    {
//        // Save file inside transaction
//        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "prescriptions");
//        if (!Directory.Exists(uploadsFolder))
//            Directory.CreateDirectory(uploadsFolder);

//        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PrescriptionFile.FileName)}";
//        filePath = Path.Combine(uploadsFolder, fileName);

//        using (var stream = new FileStream(filePath, FileMode.Create))
//        {
//            await model.PrescriptionFile.CopyToAsync(stream);
//        }

//        var prescription = new Prescription
//        {
//            CustomerId = "8a43dadf-0a54-4703-b40b-c55784374498", // TODO: Replace with logged-in user later
//            DoctorName = model.DoctorName,
//            DateIssued = DateTime.Now,
//            //TotalRepeats = model.TotalRepeats,
//            //RemainingRepeats = model.TotalRepeats,
//            TotalCost = model.Medications?.Sum(m => m.Price * m.Quantity) ?? 0,
//            FilePath = $"/uploads/prescriptions/{fileName}",
//            PrescriptionStatus = PrescriptionStatus.Pending
//        };

//        _context.Prescriptions.Add(prescription);
//        await _context.SaveChangesAsync();

//        if (model.Medications == null || !model.Medications.Any())
//            throw new InvalidOperationException("A prescription must contain at least one medication.");

//        foreach (var med in model.Medications)
//        {
//            var medicationItem = new MedicationItem
//            {
//                PrescriptionId = prescription.PrescriptionId,
//                MedicationId = med.MedicationId,
//                Dosage = med.Dosage,
//                Quantity = med.Quantity,
//                //Price = med.Price
//            };
//            _context.MedicationItems.Add(medicationItem);
//        }

//        await _context.SaveChangesAsync();
//        await transaction.CommitAsync();

//        TempData["Success"] = "Prescription uploaded successfully!";
//        return RedirectToAction("MyPrescriptions");
//    }
//    catch (Exception ex)
//    {
//        await transaction.RollbackAsync();

//        // Delete file if transaction fails
//        if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
//        {
//            System.IO.File.Delete(filePath);
//        }

//        ModelState.AddModelError("", $"Failed to upload prescription: {ex.Message}");
//        return ReloadView(model);
//    }
//}