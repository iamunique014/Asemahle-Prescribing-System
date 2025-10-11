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
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Path = System.IO.Path;

namespace PrescribingSystem.Controllers
{
    [Authorize]
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
        //[Authorize]
        public IActionResult Upload()
        {
            return View();
        }

        //POST: /Prescription/Upload
        //[Authorize]
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
                ModelState.AddModelError("PrescriptionFile", "Please select a PDF file to upload.");              
                return View(model);
            }

            if (Path.GetExtension(model.PrescriptionFile.FileName).ToLower() != ".pdf")
            {
                ModelState.AddModelError("PrescriptionFile", "Only PDF files are allowed.");
                return View(model);
            }

            if (model.PrescriptionFile.Length > 10 * 1024 * 1024)
            {
                ModelState.AddModelError("PrescriptionFile", "File size must be less than 10MB.");
                return View(model);
            }

           
            string filePath = string.Empty;

            //Check and create file explorer directory if not created yet.
            //This is where pdf prescriptions are saved
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "prescriptions");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            //Generate a Guid to use as precription filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.PrescriptionFile.FileName)}";
            filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.PrescriptionFile.CopyToAsync(stream);
            }

            string rawText = ""; //will store prescription text after reading

            //use iTextSharp pdf reader to read through the pdf.
            using (PdfReader reader = new PdfReader(filePath))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    rawText += PdfTextExtractor.GetTextFromPage(reader, i);
                }
            }

            //Map Prescription properties and save.
            var prescription = new Prescription
            {
                CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                PrescriptionDate = DateTime.UtcNow,
                TotalCost = 0,
                FilePath = $"/uploads/prescriptions/{fileName}",
                PrescriptionStatus = PrescriptionStatus.Pending,
                RawText = rawText,
                ShouldProcess = model.ShouldProcess
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = "Prescription uploaded successfully!";
            return RedirectToAction("MyPrescriptions");

        }

        // GET: /Prescription/MyPrescriptions
        public async Task<IActionResult> MyPrescriptions()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            //Get customer prescriptions
            //In Descending order so that latest prescription displays first
            var prescriptions = await _context.Prescriptions
                .Where(p => p.CustomerId == customerId && p.IsDeleted == false)
                .OrderByDescending(p => p.PrescriptionDate)
                .ToListAsync();

            return View(prescriptions);
        }

        // GET: /Prescription/PrescriptionDetails
        [HttpGet]
        public IActionResult PrescriptionDetails(int prescriptionId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            //Get a Prescription with it's medication items
            //Allows customer to view medication details including repeats
            var prescription = _context.Prescriptions
                .Include(p => p.MedicationItems)
                .ThenInclude(mi => mi.Medication)
                .FirstOrDefault(p => p.CustomerId == customerId && p.PrescriptionId == prescriptionId);

            if (prescription == null)
                return NotFound();

            //Prescription Total Cost
            // Only calculate if prescription is processed
            if (prescription.PrescriptionStatus == PrescriptionStatus.Processed)
            {
                prescription.TotalCost = prescription.MedicationItems
                    .Sum(mi => mi.Quantity * mi.Medication.CurrentSalesPrice);
            }

            return View(prescription);
        }

        // GET: /Prescription/EditPrescription/{prescriptionId}
        [HttpGet]
        public IActionResult EditPrescription(int prescriptionId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var prescription = _context.Prescriptions
                .FirstOrDefault(p => p.CustomerId == customerId && p.PrescriptionId == prescriptionId);

            if (prescription == null)
                return NotFound();

            // Map existing prescription to view model
            var model = new PrescriptionUploadViewModel
            {
                PrescriptionId = prescription.PrescriptionId,
                ShouldProcess = prescription.ShouldProcess,
                ExistingFilePath = prescription.FilePath
            };

            return View(model);
        }

        // POST: /Prescription/EditPrescription
        [HttpPost]
        public async Task<IActionResult> EditPrescription(PrescriptionUploadViewModel model)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!ModelState.IsValid)
                return View(model);

            var prescription = _context.Prescriptions
                .FirstOrDefault(p => p.CustomerId == customerId && p.PrescriptionId == model.PrescriptionId);

            if (prescription == null)
                return NotFound();

            // Handle file upload if a new file was selected
            if (model.PrescriptionFile != null && model.PrescriptionFile.Length > 0)
            {
                // Validate file type and size
                if (Path.GetExtension(model.PrescriptionFile.FileName).ToLower() != ".pdf")
                {
                    ModelState.AddModelError("PrescriptionFile", "Only PDF files are allowed.");
                    return View(model);
                }

                if (model.PrescriptionFile.Length > 10 * 1024 * 1024)
                {
                    ModelState.AddModelError("PrescriptionFile", "File size must be less than 10MB.");
                    return View(model);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "prescriptions");
                if (!Directory.Exists(uploadsFolder)) // Ensure folder exists
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}.pdf";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.PrescriptionFile.CopyToAsync(stream);
                }

                // Read text from PDF
                string rawText = "";
                using (var reader = new PdfReader(filePath))
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        rawText += PdfTextExtractor.GetTextFromPage(reader, i);
                    }
                }

                // Update file details
                prescription.FilePath = $"/uploads/prescriptions/{fileName}";
                prescription.RawText = rawText;
            }

            // Update other editable fields
            prescription.ShouldProcess = model.ShouldProcess;
            prescription.PrescriptionDate = DateTime.UtcNow; // optional if you want to track update time
            prescription.PrescriptionStatus = PrescriptionStatus.Pending;

            _context.Prescriptions.Update(prescription);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Prescription updated successfully!";
            return RedirectToAction("MyPrescriptions");
        }

        public IActionResult MyOrders()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                // User not logged in redirect to login
                return RedirectToPage("/Account/Login");
            }

            var orders = _context.PrescriptionOrders
                .Where(p => p.CustomerId == customerId);

            return View(orders);
        }

        public async Task<IActionResult> DeletePrescription(int prescriptionId)
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                // User not logged in redirect to login
                return RedirectToPage("/Account/Login");
            }

            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);

            if (prescription == null)
                return NotFound();

            prescription.IsDeleted = true;
            _context.Update(prescription);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyPrescriptions");
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

