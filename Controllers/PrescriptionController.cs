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


            TempData["Success"] = "Prescription uploaded successfully!";
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

        [HttpPost]
        public async Task<IActionResult> UpdateShouldProcess(int prescriptionId, bool shouldProcess)
        {
            //Find the prescription to be updated
            var prescription = await _context.Prescriptions.FindAsync(prescriptionId);
            if (prescription == null)
                return NotFound();

            prescription.ShouldProcess = shouldProcess;
            _context.Update(prescription);
            await _context.SaveChangesAsync();

            // go back to details view
            return RedirectToAction("MyPrescriptions"); 
        }
        ////Places Customers PrescriptionOrder
        //public IActionResult DispenseRequest(int prescriptionId)
        //{
        //    //Runs check for valid userId
        //    var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    if (string.IsNullOrEmpty(customerId))
        //    {
        //        // User not logged in → redirect to login
        //        return RedirectToPage("/Account/Login");
        //    }
                                                        
        //    //Getting prescription details so i can determine if remainingrepeats
        //    var prescription = _context.Prescriptions
        //        .Include(p => p.MedicationItems)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId && p.CustomerId == customerId);

        //    if (prescription == null)
        //    {
        //        return NotFound();
        //    }

        //    // Allow request only if at least one item still has repeats left
        //    if (!prescription.MedicationItems.Any(mi => mi.RemainingRepeats > 0))
        //    {
        //        TempData["ErrorMessage"] = "You have no repeats left for this prescription.";
        //        return RedirectToAction("PrescriptionDetails", new { prescriptionId });
        //    }

        //    var prescriptionOrder = new PrescriptionOrders
        //    {
        //        CustomerId = customerId,
        //        PrescriptionId = prescriptionId,
        //        OrderDate = DateTime.UtcNow,
        //        OrderStatus = OrderStatus.Pending,
        //        IsDeleted = IsDeleted.Active
        //    };
                
        //    _context.PrescriptionOrders.Add(prescriptionOrder);

        //    // Decrease RemainingRepeats only for items that still have repeats
        //    foreach (var item in prescription.MedicationItems.Where(mi => mi.RemainingRepeats > 0))
        //    {
        //        item.RemainingRepeats -= 1;
        //    }

        //    _context.SaveChanges();


        //    TempData["SuccessMessage"] = "Your dispensing request has been submitted.";
        //    return RedirectToAction("MyOrders");
        //}

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

