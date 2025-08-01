using iTextSharp.text;
using iTextSharp.text.pdf;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using OfficeOpenXml;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using Document = iTextSharp.text.Document;




namespace PrescribingSystem.Controllers
{
    public class PharmacyManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SmtpSettings _smtpSettings;
        private readonly UserManager<ApplicationUser> _userManager;
       

        public PharmacyManagerController(ApplicationDbContext  context, SmtpSettings smtpSettings, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _smtpSettings = smtpSettings;
            _userManager = userManager;
            
        }


        public async Task<IActionResult> NurseHome()
        {
            // Move to memory before grouping to avoid SQL translation issues
            var customerVisits = _context.Customer
                .AsEnumerable()
                .GroupBy(c => c.VisitDate.DayOfWeek)
                .Select(g => new
                {
                    Day = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            // Ensure days are ordered for the chart: Monday to Friday
            var orderedDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var chartData = orderedDays.Select(day =>
                customerVisits.FirstOrDefault(c => c.Day == day)?.Count ?? 0
            ).ToList();

            ViewBag.ChartData = chartData;

            return View(await _context.Doctor.ToListAsync());
  
        }
        public async Task<IActionResult> ManagerAsync()
        {// Move to memory before grouping to avoid SQL translation issues
            var customerVisits = _context.Customer
                .AsEnumerable()
                .GroupBy(c => c.VisitDate.DayOfWeek)
                .Select(g => new
                {
                    Day = g.Key.ToString(),
                    Count = g.Count()
                })
                .ToList();

            // Ensure days are ordered for the chart: Monday to Friday
            var orderedDays = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            var chartData = orderedDays.Select(day =>
                customerVisits.FirstOrDefault(c => c.Day == day)?.Count ?? 0
            ).ToList();

            ViewBag.ChartData = chartData;

            return View(await _context.Doctor.ToListAsync());

            //return View();
        }
        // GET: ActiveIngredient
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActiveIngredients.ToListAsync());
        }

        // GET: ActiveIngredient/Details/5
        public async Task<IActionResult> ActiveIngredientsDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredients = await _context.ActiveIngredients
                .FirstOrDefaultAsync(m => m.ActiveIngredientId == id);
            if (activeIngredients == null)
            {
                return NotFound();
            }

            return View(activeIngredients);
        }
        // GET: ActiveIngredient/Edit/5
        public async Task<IActionResult> EditActiveIngredients(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredients = await _context.ActiveIngredients.FindAsync(id);
            if (activeIngredients == null)
            {
                return NotFound();
            }
            return View(activeIngredients);
        }

        // POST: ActiveIngredient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActiveIngredients(int id, [Bind("ActiveIngredientId,ActiveIngredientName")] ActiveIngredients activeIngredients)
        {
            if (id != activeIngredients.ActiveIngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activeIngredients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiveIngredientsExists(activeIngredients.ActiveIngredientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activeIngredients);
        }

        // GET: ActiveIngredient/Delete/5
        public async Task<IActionResult> DeleteActiveIngredients(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredients = await _context.ActiveIngredients
                .FirstOrDefaultAsync(m => m.ActiveIngredientId == id);
            if (activeIngredients == null)
            {
                return NotFound();
            }

            return View(activeIngredients);
        }

        // POST: ActiveIngredient/Delete/5
        [HttpPost, ActionName("DeleteActiveIngredients")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activeIngredients = await _context.ActiveIngredients.FindAsync(id);
            if (activeIngredients != null)
            {
                _context.ActiveIngredients.Remove(activeIngredients);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
        public IActionResult ManageIngredients()
        {
            var viewModel = new ActiveIngredientViewModel
            {
                ActiveIngredientsList = _context.ActiveIngredients.ToList()
            };

            return View(viewModel);
        }
        // GET: ActiveIngredient/Create
        public IActionResult AddActiveIngredient()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActiveIngredient([Bind("ActiveIngredientId,ActiveIngredientName")] ActiveIngredients activeIngredients)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activeIngredients);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activeIngredients);
        }

        // GET: ActiveIngredient/Edit/5
        public async Task<IActionResult> EditActiveIngredient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredients = await _context.ActiveIngredients.FindAsync(id);
            if (activeIngredients == null)
            {
                return NotFound();
            }
            return View(activeIngredients);
        }

        // POST: ActiveIngredient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActiveIngredient(int id, [Bind("ActiveIngredientId,ActiveIngredientName")] ActiveIngredients activeIngredients)
        {
            if (id != activeIngredients.ActiveIngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activeIngredients);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiveIngredientsExists(activeIngredients.ActiveIngredientId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activeIngredients);
        }

        // GET: ActiveIngredient/Delete/5
        public async Task<IActionResult> DeleteActiveIngredient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredient = await _context.ActiveIngredients
                .FirstOrDefaultAsync(m => m.ActiveIngredientId == id);

            if (activeIngredient == null)
            {
                return NotFound();
            }

            return View(activeIngredient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActiveIngredient(int id)
        {
            var activeIngredient = await _context.ActiveIngredients.FindAsync(id);
            if (activeIngredient != null)
            {
                _context.ActiveIngredients.Remove(activeIngredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        //End of Active Ingredient//
        /// <summary>
        /// Above is the Active Ingredient Model
        /// </summary>
        /// <returns>
        /// </returns>
        //Add doctors//

        // GET: doctors
        public async Task<IActionResult> IndexDoctor()
        {
            return View(await _context.Doctor.ToListAsync());
        }
        public IActionResult ExportStockOrderToPdf(int id)
        {
            var order = _context.StockOrder
                                .Include(o => o.MedicationStockOrder)
                                    .ThenInclude(mso => mso.Medication)
                                .FirstOrDefault(o => o.StockOrderId == id);

            if (order == null)
                return NotFound();

            using (var stream = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var writer = PdfWriter.GetInstance(document, stream);
                writer.CloseStream = false;

                document.Open();
                document.Add(new Paragraph("Medication Stock Order Report\n\n"));
                document.Add(new Paragraph($"Order Number: {order.OrderNumber}"));
                document.Add(new Paragraph($"Order Date: {order.OrderDate.ToShortDateString()}"));
                document.Add(new Paragraph($"Status: {(order.Status ? "Processed" : "Pending")}\n\n"));

                document.Add(new Paragraph("Medications:\n"));

                foreach (var item in order.MedicationStockOrder)
                {
                    var medName = item.Medication?.Name ?? "N/A";
                    document.Add(new Paragraph($"- {medName}, Quantity: {item.Quantity}"));
                }

                document.Close();
                var bytes = stream.ToArray();

                return File(bytes, "application/pdf", $"StockOrder_{order.OrderNumber}.pdf");
            }
        }

        public IActionResult ExportDoctorsToPdf()
        {
            var doctors = _context.Doctor.ToList();

            using (var stream = new MemoryStream())
            {
                var document = new Document();
                var writer = PdfWriter.GetInstance(document, stream);
                writer.CloseStream = false; // Prevent iTextSharp from closing the stream

                document.Open();
                document.Add(new Paragraph("Doctor Report\n\n"));

                foreach (var doc in doctors)
                {
                    document.Add(new Paragraph($"Name: {doc.Name} {doc.Surname}"));
                    document.Add(new Paragraph($"Specialty: {doc.Practice_Number}"));
                    document.Add(new Paragraph($"Email: {doc.Email}\n\n"));
                }

                document.Close();

                // Return byte array instead of passing stream directly
                var bytes = stream.ToArray();
                return File(bytes, "application/pdf", "Doctors.pdf");
            }
        }

        public IActionResult ExportDoctorsToExcel()
        {
            var doctors = _context.Doctor.ToList();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Doctors");

                // Header
                worksheet.Cells[1, 1].Value = "First Name";
                worksheet.Cells[1, 2].Value = "Last Name";
                worksheet.Cells[1, 3].Value = "Specialty";
                worksheet.Cells[1, 4].Value = "Email";

                // Data
                for (int i = 0; i < doctors.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = doctors[i].Name;
                    worksheet.Cells[i + 2, 2].Value = doctors[i].Surname;
                    worksheet.Cells[i + 2, 3].Value = doctors[i].Practice_Number;
                    worksheet.Cells[i + 2, 4].Value = doctors[i].Email;
                }

                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doctors.xlsx");
            }
        }
        // GET: ActiveIngredient/Details/5
        public async Task<IActionResult> DetailsDoctor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredients = await _context.Doctor
                .FirstOrDefaultAsync(m => m.DoctorId == id);
            if (activeIngredients == null)
            {
                return NotFound();
            }

            return View(activeIngredients);
        }

        // GET: ActiveIngredient/Create
        public IActionResult AddDoctor()
        {
            return View();
        }

        // POST: ActiveIngredient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(Doctor Doctor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexDoctor));
            }
            return View(Doctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDeleteDoctor(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            // Move to DeletedDoctor table
            var deletedDoctor = new DeletedDoctor
            {
                Name = doctor.Name,
                Surname = doctor.Surname,
                Practice_Number = doctor.Practice_Number,
                Email = doctor.Email
            };

            _context.DeletedDoctors.Add(deletedDoctor);
            _context.Doctor.Remove(doctor);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Doctor successfully removed and archived.";
            return RedirectToAction(nameof(IndexDoctor));
        }


        // GET: ActiveIngredient/Edit/5
        public async Task<IActionResult> EditDoctor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activeIngredients = await _context.Doctor.FindAsync(id);
            if (activeIngredients == null)
            {
                return NotFound();
            }
            return View(activeIngredients);
        }

        // POST: ActiveIngredient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(int id, Doctor Doctor)
        {
            if (id != Doctor.DoctorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActiveIngredientsExists(Doctor.DoctorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexDoctor));
            }
            return View(Doctor);
        }

        // GET: Doctor/Delete/5
        public async Task<IActionResult> DeleteDoctor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctor.FirstOrDefaultAsync(m => m.DoctorId == id);

            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("DeleteDoctor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctorConfirmed(int id)
        {
            var doctor = await _context.Doctor.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctor.Remove(doctor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(IndexDoctor));
        }
        
        public async Task<IActionResult> IndexCustomerAllergy()
        {
            var allergies = _context.CustomerAllergy
                .Include(c => c.Customer)
                .Include(c => c.ActiveIngredients);
            return View(await allergies.ToListAsync());
        }

        public IActionResult AddCustomerAllergy()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name");
            ViewData["ActiveIngredientId"] = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "ActiveIngredientName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCustomerAllergy(CustomerAllergy customerAllergy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerAllergy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", customerAllergy.CustomerId);
            ViewData["ActiveIngredientId"] = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "ActiveIngredientName", customerAllergy.ActiveIngredientId);
            return View(customerAllergy);
        }

        public async Task<IActionResult> EditCustomerAllergy(int? id)
        {
            if (id == null)
                return NotFound();

            var customerAllergy = await _context.CustomerAllergy.FindAsync(id);
            if (customerAllergy == null)
                return NotFound();

            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", customerAllergy.CustomerId);
            ViewData["ActiveIngredientId"] = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "ActiveIngredientName", customerAllergy.ActiveIngredientId);
            return View(customerAllergy);
        }

          [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomerAllergy(int id, CustomerAllergy customerAllergy)
        {
            if (id != customerAllergy.CustomerAllergyId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerAllergy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerAllergyExists(customerAllergy.CustomerAllergyId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", customerAllergy.CustomerId);
            ViewData["ActiveIngredientId"] = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "ActiveIngredientName", customerAllergy.ActiveIngredientId);
            return View(customerAllergy);
        }

        public async Task<IActionResult> DetailsCustomerAllergy(int? id)
        {
            if (id == null)
                return NotFound();

            var allergy = await _context.CustomerAllergy
                .Include(c => c.Customer)
                .Include(c => c.ActiveIngredients)
                .FirstOrDefaultAsync(m => m.CustomerAllergyId == id);

            if (allergy == null)
                return NotFound();

            return View(allergy);
        }

        public async Task<IActionResult> DeleteCustomerAllergy(int? id)
        {
            if (id == null)
                return NotFound();

            var allergy = await _context.CustomerAllergy
                .Include(c => c.Customer)
                .Include(c => c.ActiveIngredients)
                .FirstOrDefaultAsync(m => m.CustomerAllergyId == id);

            if (allergy == null)
                return NotFound();

            return View(allergy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCustomerAllergy(int id)
        {
            var allergy = await _context.CustomerAllergy.FindAsync(id);
            _context.CustomerAllergy.Remove(allergy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: DorsageForm
        public async Task<IActionResult> IndexDorsageForm()
        {
            return View(await _context.DorsageForm.ToListAsync());
        }

        // GET: DorsageForm/Create
        public IActionResult AddDorsageForm()
        {
            return View();
        }

        // POST: DorsageForm/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDorsageForm(DorsageForm dorsageForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dorsageForm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexDorsageForm));
            }
            return View(dorsageForm);
        }

        // GET: DorsageForm/Edit/5
        public async Task<IActionResult> DorsageFormEdit(int? id)
        {
            if (id == null) return NotFound();

            var dorsageForm = await _context.DorsageForm.FindAsync(id);
            if (dorsageForm == null) return NotFound();

            return View(dorsageForm);
        }

        // POST: DorsageForm/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DorsageFormEdit(int id, DorsageForm dorsageForm)
        {
            if (id != dorsageForm.DorsageFormId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dorsageForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DorsageForm.Any(e => e.DorsageFormId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(IndexDorsageForm));
            }
            return View(dorsageForm);
        }

        // GET: DorsageForm/Delete/5
        public async Task<IActionResult> DeleteDorsage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dorsageform = await _context.DorsageForm
                .FirstOrDefaultAsync(m => m.DorsageFormId == id);
            if (dorsageform == null)
            {
                return NotFound();
            }

            return View(dorsageform);
        }

        // POST: ActiveIngredient/Delete/5
        [HttpPost, ActionName("DeleteDorsage")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDorsageConfirmed(int id)
        {
            var dorsage = await _context.DorsageForm.FindAsync(id);
            if (dorsage != null)
            {
                _context.DorsageForm.Remove(dorsage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexDorsageForm));
        }
        
        public async Task<IActionResult> IndexPharmacist()
        {
            return View(await _context.Pharmacist.ToListAsync());
        }

        // GET: Pharmacist/Details/5
        public async Task<IActionResult> PharmacistDetails(int? id)
        {
            if (id == null) return NotFound();

            var pharmacist = await _context.Pharmacist
                .FirstOrDefaultAsync(m => m.PharmacistId == id);

            if (pharmacist == null) return NotFound();

            return View(pharmacist);
        }

        // GET: Pharmacist/Create
        public IActionResult AddPharmacist()
        {
            return View();
        }

        // POST: Pharmacist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPharmacist(Pharmacist pharmacist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pharmacist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPharmacist));
            }
            return View(pharmacist);
        }

        // GET: Pharmacist/Edit/5
        public async Task<IActionResult> EditPharmacist(int? id)
        {
            if (id == null) return NotFound();

            var pharmacist = await _context.Pharmacist.FindAsync(id);
            if (pharmacist == null) return NotFound();
            return View(pharmacist);
        }

        // POST: Pharmacist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPharmacist(int id, [Bind("PharmacistId,Name,Surname,Cellphone,CellphoneId,Email,NewPassword,Password,HealthCouncil,RegistrationNumber")] Pharmacist pharmacist)
        {
            if (id != pharmacist.PharmacistId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacistExists(pharmacist.PharmacistId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(IndexPharmacist));
            }
            return View(pharmacist);
        }
        // GET: PharmacyManager/DetailsPharmacist/5
        public async Task<IActionResult> DetailsPharmacist(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacist = await _context.Pharmacist
                .FirstOrDefaultAsync(m => m.PharmacistId == id);

            if (pharmacist == null)
            {
                return NotFound();
            }

            return View(pharmacist);
        }

        // GET: Pharmacist/Delete/5
        public async Task<IActionResult> DeletePharmacist(int? id)
        {
            if (id == null) return NotFound();

            var pharmacist = await _context.Pharmacist
                .FirstOrDefaultAsync(m => m.PharmacistId == id);
            if (pharmacist == null) return NotFound();

            return View(pharmacist);
        }

        // POST: Pharmacist/Delete/5
        [HttpPost, ActionName("DeletePharmacist")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PharmacistDeleteConfirmed(int id)
        {
            var pharmacist = await _context.Pharmacist.FindAsync(id);
            _context.Pharmacist.Remove(pharmacist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexPharmacist));

        }
        
        public async Task<IActionResult> IndexPharmacy()
        {
            var pharmacies = await _context.Pharmacy
                                   .Include(p => p.Pharmacist)
                                   .ToListAsync();

            return View(pharmacies);
        }


        public IActionResult CreatePharmacy()
        {
            ViewBag.Pharmacist = new SelectList(
                _context.Pharmacist
                    .Select(p => new
                    {
                        p.PharmacistId,
                        FullInfo = p.Name + " " + p.Surname + " (" + p.RegistrationNumber + ")"
                    }),
                "PharmacistId",
                "FullInfo"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePharmacy(Pharmacy pharmacy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pharmacy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexPharmacy));
            }

            ViewBag.Pharmacist = new SelectList(
                _context.Pharmacist
                    .Select(p => new
                    {
                        p.PharmacistId,
                        FullInfo = p.Name + " " + p.Surname + " (" + p.RegistrationNumber + ")"
                    }),
                "PharmacistId",
                "FullInfo",
                pharmacy.PharmacistId
            );
          

            return View(pharmacy);
        }

        // GET: Pharmacy/Edit/5
        public async Task<IActionResult> EditPharmacy(int? id)
        {
            if (id == null) return NotFound();

            var pharmacy = await _context.Pharmacy.FindAsync(id);
            if (pharmacy == null) return NotFound();

            // Populate ViewBag with pharmacists' full names and registration numbers
            ViewBag.Pharmacist = new SelectList(
                _context.Pharmacist
                    .Select(p => new {
                        PharmacistId = p.PharmacistId,
                        FullDisplay = p.Name + " " + p.Surname + " - " + p.RegistrationNumber
                    }),
                "PharmacistId",
                "FullDisplay",
                pharmacy.PharmacistId
            );

            return View(pharmacy);
        }

        // POST: Pharmacy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPharmacy(int id, Pharmacy pharmacy)
        {
            if (id != pharmacy.PharmacyId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Pharmacy.Any(e => e.PharmacyId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Repopulate ViewBag with pharmacists' full names and registration numbers in case of a validation error
            ViewBag.Pharmacist = new SelectList(
                _context.Pharmacist
                    .Select(p => new {
                        PharmacistId = p.PharmacistId,
                        FullDisplay = p.Name + " " + p.Surname + " - " + p.RegistrationNumber
                    }),
                "PharmacistId",
                "FullDisplay",
                pharmacy.PharmacistId
            );

            return View(pharmacy);
        }


        // GET: Pharmacy/Delete/5
        public async Task<IActionResult> DeletePharmacy(int? id)
        {
            if (id == null) return NotFound();

            var pharmacy = await _context.Pharmacy.FirstOrDefaultAsync(m => m.PharmacyId == id);

            if (pharmacy == null) return NotFound();

            return View(pharmacy);
        }

        // POST: Pharmacy/Delete/5
        [HttpPost, ActionName("DeletePharmacy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PharmacyDeleteConfirmed(int id)
        {
            var pharmacy = await _context.Pharmacy.FindAsync(id);
            _context.Pharmacy.Remove(pharmacy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Pharmacy/Details/5
        public async Task<IActionResult> PharmacyDetails(int? id)
        {
            if (id == null) return NotFound();

            var pharmacy = await _context.Pharmacy.FirstOrDefaultAsync(m => m.PharmacyId == id);

            //.Include(p => p.Pharmacist)

            if (pharmacy == null) return NotFound();

            return View(pharmacy);
        }
        // GET: MedicationSuppliers
        public async Task<IActionResult> IndexSupplier()
        {
            return View(await _context.Supplier.ToListAsync());
        }

        // GET: MedicationSuppliers/Create
        public IActionResult AddSupplier()
        {
            return View();
        }

        // POST: MedicationSuppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSupplier([Bind("SupplierName,ContactPerson,Email")] Supplier medicationSupplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicationSupplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexSupplier));
            }
            return View(medicationSupplier);
        }

        // GET: MedicationSuppliers/Edit/5
        public async Task<IActionResult> SupplierEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationSupplier = await _context.Supplier.FindAsync(id);
            if (medicationSupplier == null)
            {
                return NotFound();
            }
            return View(medicationSupplier);
        }

        // POST: MedicationSuppliers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupplierEdit(int id, [Bind("SupplierId,SupplierName,ContactPerson,Email")] Supplier medicationSupplier)
        {
            if (id != medicationSupplier.SupplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicationSupplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(medicationSupplier.SupplierId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicationSupplier);
        }
        // GET: MedicationSupplier/Details/5
        public async Task<IActionResult> SupplierDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.SupplierId == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: MedicationSuppliers/Delete/5
        public async Task<IActionResult> DeleteSupplier(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicationSupplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (medicationSupplier == null)
            {
                return NotFound();
            }

            return View(medicationSupplier);
        }

        // POST: MedicationSuppliers/Delete/5
        [HttpPost, ActionName("DeleteSupplier")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupplierDeleteConfirmed(int id)
        {
            var medicationSupplier = await _context.Supplier.FindAsync(id);
            _context.Supplier.Remove(medicationSupplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexSupplier));
        }
        // GET: Medication/Index
       
        public async Task<IActionResult> IndexMedication()
        {
            var medications = await _context.Medication
                .Include(m => m.DorsageForm)
                .Include(m => m.Supplier)
                .Include(m => m.MedicationActiveIngredients)
                    .ThenInclude(ma => ma.ActiveIngredient)
                .ToListAsync();

            return View(medications);
        }



        // GET: Medication/Create
        public IActionResult AddMedication()
        {
            // Populate dropdowns using ViewBag
            ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName");
            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName");
            // Populate the Dosage Form dropdown with values from the database
            //ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName");
            // Send the actual list of active ingredients
            ViewBag.ActiveIngredients = _context.ActiveIngredients.ToList();
            //// Populate the Supplier dropdown with values from the database
            //ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName");

            return View();
        }

        // POST: Medication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedication(MedicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var medication = new Medication
                {
                    Name = model.Name,
                    Schedule = model.Schedule,
                    DorsageFormId = model.DorsageFormId,
                    CurrentSalesPrice = model.CurrentSalesPrice,
                    SupplierId = model.SupplierId,
                    ReOrderLevel = model.ReOrderLevel,
                    QuantityOnHand = model.QuantityOnHand,
                    Status = model.Status,
                };

                // Add associated active ingredients and strengths
                foreach (var activeIngredientId in model.SelectedActiveIngredientIds)
                {
                    if (model.ActiveIngredientStrengths.TryGetValue(activeIngredientId, out var strengthStr)
                        && decimal.TryParse(strengthStr, out decimal strength))
                    {
                        medication.MedicationActiveIngredients.Add(new MedicationActiveIngredient
                        {
                            ActiveIngredientId = activeIngredientId,
                            Strength = strength
                        });
                    }
                }

                _context.Medication.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexMedication");
            }

            // If model is invalid, re-populate dropdowns
            ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName");
            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName");
            ViewBag.ActiveIngredients = _context.ActiveIngredients.ToList();

            return View(model);
        }
        // GET: Medication/Edit/5
        public async Task<IActionResult> EditMedication(int? id)
        {
            if (id == null) return NotFound();

            var medication = await _context.Medication
                .Include(m => m.MedicationActiveIngredients)
                .FirstOrDefaultAsync(m => m.MedicationId == id);

            if (medication == null) return NotFound();

            ViewBag.DorsageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName", medication.DorsageFormId);
            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", medication.SupplierId);
            ViewBag.ActiveIngredients = new MultiSelectList(
                _context.ActiveIngredients,
                "ActiveIngredientId",
                "ActiveIngredientName",
                medication.MedicationActiveIngredients.Select(x => x.ActiveIngredientId)
            );

            return View(medication);
        }


        // POST: Medication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedication(int id, Medication medication, int[] selectedIngredients)
        {
            if (id != medication.MedicationId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Update Medication table
                    _context.Update(medication);

                    // Remove old ingredients
                    var existingIngredients = _context.MedicationActiveIngredient
                        .Where(m => m.MedicationId == id);
                    _context.MedicationActiveIngredient.RemoveRange(existingIngredients);

                    // Add new ingredients
                    foreach (var ingredientId in selectedIngredients)
                    {
                        _context.MedicationActiveIngredient.Add(new MedicationActiveIngredient
                        {
                            MedicationId = medication.MedicationId,
                            ActiveIngredientId = ingredientId
                        });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Medication.Any(e => e.MedicationId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("IndexMedication");
            }

            // Repopulate dropdowns
            ViewBag.DorsageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName", medication.DorsageFormId);
            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", medication.SupplierId);
            ViewBag.ActiveIngredients = new MultiSelectList(_context.ActiveIngredients, "ActiveIngredientId", "ActiveIngredientName", selectedIngredients);

            return View(medication);
        }

        // GET: Medication/Details/5
        public async Task<IActionResult> MedicationDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medication
                .Include(m => m.DorsageForm)
                .Include(m => m.Supplier)
                .Include(m => m.MedicationActiveIngredients)
                    .ThenInclude(ma => ma.ActiveIngredient)
                .FirstOrDefaultAsync(m => m.MedicationId == id);

            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

       
        public async Task<IActionResult> DeleteMedication(int? id)
        {
            if (id == null)
                return NotFound();

            var medication = await _context.Medication
                .Include(m => m.DorsageForm)
                .Include(m => m.Supplier)
                .FirstOrDefaultAsync(m => m.MedicationId == id);

            if (medication == null)
                return NotFound();

            return View(medication);
        }
        // POST: ActiveIngredient/Delete/5
        [HttpPost, ActionName("DeleteMedication")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicationDeleteConfirmed(int id)
        {
            var medication = await _context.Medication
         .Include(m => m.MedicationActiveIngredients)
         .Include(m => m.MedicationActiveIngredients)
         .Include(m => m.MedicationStockOrder)
         .FirstOrDefaultAsync(m => m.MedicationId == id);

            if (medication == null)
                return NotFound();

            // Remove related MedicationStockOrder entries
            var relatedStockOrders = _context.MedicationStockOrder
                .Where(mso => mso.MedicationId == id);
            _context.MedicationStockOrder.RemoveRange(relatedStockOrders);

            // Optionally remove related many-to-many MedicationActiveIngredients
            _context.MedicationActiveIngredient.RemoveRange(medication.MedicationActiveIngredients);

            _context.Medication.Remove(medication);
            await _context.SaveChangesAsync();

            return RedirectToAction("IndexMedication");
        }
        public IActionResult PreviewStockOrder(int id)
        {
            var order = _context.StockOrder
                .Include(o => o.Supplier)
                .Include(o => o.MedicationStockOrder)
                .ThenInclude(mso => mso.Medication)
                .FirstOrDefault(o => o.StockOrderId == id);

            if (order == null)
                return NotFound();

            return View(order); // Make sure you have Views/StockOrder/PreviewStockOrder.cshtml
        }
       
        public async Task<IActionResult> ListApprovedOrders()
        {
            var approvedOrders = await _context.ApprovedOrders
                .Include(a => a.MedicationItems)
                .ToListAsync();

            return View(approvedOrders);
        }
        

        [HttpPost]
        public async Task<IActionResult> MarkOrderAsReceived(int id)
        {
            var approvedOrder = await _context.ApprovedOrders.FindAsync(id);

            if (approvedOrder == null)
            {
                TempData["ErrorMessage"] = "Approved order not found.";
                return RedirectToAction("ApprovedOrders");
            }

            approvedOrder.IsReceived = true;
            approvedOrder.ReceivedAt = DateTime.Now;

            _context.Update(approvedOrder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Order {approvedOrder.OrderNumber} marked as received.";
            return RedirectToAction("ApprovedOrders");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitGroupedMedicationStock(List<SubmitGroupedMedicationViewModel> groupedMedications)
        {
            foreach (var group in groupedMedications)
            {
                var medications = await _context.Medication
                    .Where(m => group.MedicationsIds.Contains(m.MedicationId))
                    .ToListAsync();

                var orderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmssfff}-{group.SupplierId}";

                var stockOrder = new StockOrder
                {
                    SupplierId = group.SupplierId,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now,
                    Status = false
                };

                _context.StockOrder.Add(stockOrder);
                await _context.SaveChangesAsync();

                foreach (var med in medications)
                {
                    _context.MedicationStockOrder.Add(new MedicationStockOrder
                    {
                        StockOrderId = stockOrder.StockOrderId,
                        MedicationId = med.MedicationId,
                        Quantity = 0
                    
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ListMedicationStock");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmMedicationStock(StockOrderCreateViewModel viewModel)
        {
            if (viewModel.SelectedMedicationIds == null || viewModel.SelectedMedicationIds.Count == 0)
            {
                ModelState.AddModelError("", "Please select at least one medication.");

                viewModel.Medications = await _context.Medication
                    .Include(m => m.DorsageForm)
                    .Include(m => m.Supplier)
                    .ToListAsync();

                viewModel.Supplier = await _context.Supplier
                    .FirstOrDefaultAsync(s => s.SupplierId == viewModel.SupplierId);

                return View("AddMedicationStock", viewModel);
            }

            // Fetch selected medications
            var selectedMedications = await _context.Medication
                .Where(m => viewModel.SelectedMedicationIds.Contains(m.MedicationId))
                .Include(m => m.DorsageForm)
                .Include(m => m.Supplier)
                .ToListAsync();

            // Assign selected medications and supplier
            viewModel.Medications = selectedMedications;
            viewModel.Supplier = await _context.Supplier
                .FirstOrDefaultAsync(s => s.SupplierId == viewModel.SupplierId);

            return View("ConfirmGroupedMedicationStock", viewModel);
        }


        public async Task<IActionResult> IndexStockOrder(DateTime? startDate, DateTime? endDate, string searchOrderNumber)
        {
            ViewBag.CurrentFilter = searchOrderNumber;

            // Get pending orders
            var pendingOrdersQuery = _context.StockOrder
                .Include(s => s.Supplier)
                .Include(s => s.MedicationStockOrder)
                    .ThenInclude(i => i.Medication)
                .Where(o => !o.Status)
                .AsQueryable();

            // Apply filters to pending orders
            if (startDate.HasValue)
                pendingOrdersQuery = pendingOrdersQuery.Where(o => o.OrderDate >= startDate.Value.Date);

            if (endDate.HasValue)
                pendingOrdersQuery = pendingOrdersQuery.Where(o => o.OrderDate <= endDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(searchOrderNumber))
                pendingOrdersQuery = pendingOrdersQuery.Where(o =>
                    o.OrderNumber.ToLower().Contains(searchOrderNumber.Trim().ToLower()));

            var pendingOrders = await pendingOrdersQuery.ToListAsync();


            // Get approved orders
            var approvedOrdersQuery = _context.ApprovedOrders
                .Include(a => a.MedicationItems)
                .AsQueryable();

            // Apply filters to approved orders
            if (startDate.HasValue)
                approvedOrdersQuery = approvedOrdersQuery.Where(a => a.OrderDate >= startDate.Value.Date);

            if (endDate.HasValue)
                approvedOrdersQuery = approvedOrdersQuery.Where(a => a.OrderDate <= endDate.Value.Date);

            if (!string.IsNullOrWhiteSpace(searchOrderNumber))
                approvedOrdersQuery = approvedOrdersQuery.Where(a =>
                    a.OrderNumber.ToLower().Contains(searchOrderNumber.Trim().ToLower()));

            var approvedOrders = await approvedOrdersQuery.ToListAsync();

            var viewModel = Tuple.Create<IEnumerable<StockOrder>, IEnumerable<ApprovedOrder>>(pendingOrders, approvedOrders);
            return View(viewModel);
        }
        public async Task<IActionResult> ApprovedOrders(string filter, string search)
        {
            var approvedOrders = await _context.ApprovedOrders
                .Include(o => o.MedicationItems)
                .ToListAsync();

            // Search by OrderNumber (case-insensitive)
            if (!string.IsNullOrWhiteSpace(search))
            {
                approvedOrders = approvedOrders
                    .Where(o => o.OrderNumber.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var viewModel = new ApprovedOrderListViewModel
            {
                ReceivedOrders = approvedOrders.Where(o => o.IsReceived).ToList(),
                NotReceivedOrders = approvedOrders.Where(o => !o.IsReceived).ToList()
            };

            ViewBag.Filter = filter;
            ViewBag.Search = search;

            return View(viewModel);
        }

        //public async Task<IActionResult> ApprovedOrders(string filter)
        //{
        //    var approvedOrders = await _context.ApprovedOrders
        //        .Include(o => o.MedicationItems)
        //        .ToListAsync();

        //    var viewModel = new ApprovedOrderListViewModel
        //    {
        //        ReceivedOrders = approvedOrders.Where(o => o.IsReceived).ToList(),
        //        NotReceivedOrders = approvedOrders.Where(o => !o.IsReceived).ToList()
        //    };

        //    ViewBag.Filter = filter;

        //    return View(viewModel);
        //}

        [HttpPost]
        public async Task<IActionResult> ApproveStockOrder2(int id)
        {
            var pharmacy = await _context.Pharmacy
    .Include(p => p.Pharmacist)
    .FirstOrDefaultAsync();
            // Get current user
            var currentUser = await _userManager.GetUserAsync(User);
            var fullName = currentUser != null
                ? $"{currentUser.FirstName} {currentUser.LastName}".Trim()
                : "System";

            // Fetch the stock order and related data
            var order = await _context.StockOrder
                .Include(o => o.MedicationStockOrder)
                    .ThenInclude(mso => mso.Medication)
                .Include(o => o.Supplier)
                .FirstOrDefaultAsync(o => o.StockOrderId == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Stock order not found.";
                return RedirectToAction("IndexStockOrder");
            }

            if (order.Status)
            {
                TempData["ErrorMessage"] = "Order already approved.";
                return RedirectToAction("IndexStockOrder");
            }

            try
            {
                // Approve the order
                order.Status = true;
                order.ApprovedBy = fullName;
                order.ApprovedAt = DateTime.Now;
                order.ReceivedDate= DateTime.Now;
                

                // Update medication quantities
                foreach (var item in order.MedicationStockOrder)
                {
                    item.Medication.QuantityOnHand += item.Quantity;
                }

                // Log approved order
                var approvedOrder = new ApprovedOrder
                {
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    ApprovedBy = fullName,
                    ApprovedDate = DateTime.Now, // ✅ set explicitly
                    ApprovedAt = DateTime.Now,   // optionally keep both
                    MedicationItems = order.MedicationStockOrder.Select(m => new ApprovedMedicationItem
                    {
                        MedicationName = m.Medication.Name,
                        Quantity = m.Quantity
                    }).ToList()
                };


                _context.ApprovedOrders.Add(approvedOrder);

                await _context.SaveChangesAsync();

                // Send confirmation email
                if (!string.IsNullOrEmpty(order.Supplier?.Email))
                {
                    try
                    {
                        var medicationList = order.MedicationStockOrder
    .Select(m => (m.Medication.Name, m.Quantity))
    .ToList();

                        await SendApprovalConfirmationEmailAsync(toEmail: "supplier@example.com",
    stockOrderNumber: "ORD-1234",
    supplierName: "ABC Med Supplies",
    medications: medicationList,
    pharmacy: pharmacy);

                    }
                    catch (Exception ex)
                    {
                        TempData["WarningMessage"] = $"Order approved, but failed to send email: {ex.Message}";
                    }
                }
                else
                {
                    TempData["WarningMessage"] = "Order approved, but supplier email not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error while approving stock order: {ex.Message}";
            }

            return RedirectToAction("IndexStockOrder");
        }


        // GET: StockOrders/Create
        public IActionResult AddStock()
        {
            var viewModel = new StockOrderViewModel
            {
                OrderDate = DateTime.Now,
                OrderNumber = $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}"
            };

            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName");
            return View(viewModel);
        }

        // POST: StockOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStock(StockOrderViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var stockOrder = new StockOrder
                {
                    OrderNumber = viewModel.OrderNumber ?? $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    /*SupplierId = viewModel.SupplierId*/
                    OrderDate = viewModel.OrderDate,
                    Status = viewModel.Status
                };

                _context.Add(stockOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexStock)); // Adjust if needed
            }

            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", viewModel.SupplierId);
            return View(viewModel);
        }

        public IActionResult AddMedicationStock()
        {
            var viewModel = new StockOrderCreateViewModel
            {
                OrderNumber = $"ORD-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                Medications = _context.Medication
                    .Include(m => m.DorsageForm)
                    .Include(m => m.Supplier)
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicationStock(AddMedicationStockViewModel viewModel)
        {
            if (viewModel.SelectedMedicationIds == null || !viewModel.SelectedMedicationIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one medication.");
                viewModel.Medications = await _context.Medication
                    .Include(m => m.DorsageForm)
                    .Include(m => m.Supplier)
                    .ToListAsync();
                viewModel.Suppliers = new SelectList(_context.Supplier, "SupplierId", "SupplierName");
                return View(viewModel);
            }

            // Fetch selected medications
            var selectedMedications = await _context.Medication
                .Where(m => viewModel.SelectedMedicationIds.Contains(m.MedicationId))
                .Include(m => m.Supplier)
                .ToListAsync();

            // Group medications by SupplierId
            var groupedBySupplier = selectedMedications
                .GroupBy(m => m.SupplierId)
                .ToList();

            foreach (var group in groupedBySupplier)
            {
                var supplierId = group.Key;
                var orderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmssfff}-{supplierId}";

                var stockOrder = new StockOrder
                {
                    SupplierId = supplierId,
                    OrderNumber = orderNumber,
                    OrderDate = DateTime.Now,
                    Status = false
                };

                _context.StockOrder.Add(stockOrder);
                await _context.SaveChangesAsync(); // Save to get StockOrderId

                foreach (var medication in group)
                {
                    var medicationStockOrder = new MedicationStockOrder
                    {
                        StockOrderId = stockOrder.StockOrderId,
                        MedicationId = medication.MedicationId,
                        OrderNumber = stockOrder.OrderNumber,
                        Quantity = viewModel.Quantity.TryGetValue(medication.MedicationId, out var qty) ? qty : 0
                    };

                    _context.MedicationStockOrder.Add(medicationStockOrder);
                }

            }

            await _context.SaveChangesAsync();

            return RedirectToAction("IndexStockOrder");
        }

        private async Task SendApprovalConfirmationEmailAsync(string toEmail,
    string stockOrderNumber,
    string supplierName,
    List<(string MedicationName, int Quantity)> medications,
    Pharmacy pharmacy)
        {
            var medicationDetails = new StringBuilder();

            foreach (var med in medications)
            {
                medicationDetails.AppendLine($"<li>{med.MedicationName}: {med.Quantity}</li>");
            }

            string logoUrl = "http://localhost:5000/images/approved.png";
            // ✅ Hosted logo
            string stampUrl = "https://images.app.goo.gl/6VubmsV1rtpXZjtp9"; // ✅ Hosted stamp

            string body = $@"
<html>
<head>
     <style>
        body {{
            font-family: Arial, sans-serif;
        }}
        .header {{
            display: flex;
            align-items: center;
            justify-content: space-between;
        }}
        .stamp {{
            float: right;
            width: 120px;
        }}
        .logo {{
            float: left;
            width: 150px;
        }}
    </style>
</head>
<body>
     <div class='header'>
        <img src='{logoUrl}' class='logo' alt='Ibhayi Pharmacy Logo' />
        <img src='{stampUrl}' class='stamp' alt='Approved Stamp' />
    </div>
    
    <p>Dear {supplierName},</p>

    <p><strong>Stock order #{stockOrderNumber}</strong> has been <strong>approved</strong>.</p>

    <p><u>Ordered Medications:</u></p>
    <ul>
        {medicationDetails}
    
    </ul>

   <p>Regards,<br />
<strong>{pharmacy.Name}</strong><br />
Registration No: {pharmacy.HealthCouncilRegistrationNumber}<br />
Address: {pharmacy.PhysicalAddress1}{(string.IsNullOrWhiteSpace(pharmacy.PhysicalAddress2) ? "" : ", " + pharmacy.PhysicalAddress2)}<br />
Phone: {pharmacy.ContactNumber}<br />
Email: {pharmacy.Email}<br />
{(string.IsNullOrWhiteSpace(pharmacy.WebsiteUrl) ? "" : $"Website: <a href='{pharmacy.WebsiteUrl}'>{pharmacy.WebsiteUrl}</a><br />")}
</p>

</body>
</html>";

            var mail = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, "Ibhayi Pharmacy System"),
                Subject = $"Stock Order {stockOrderNumber} Approved",
                Body = body,
                IsBodyHtml = true // ✅ Enable HTML
            };

            mail.To.Add(toEmail);

            using var smtp = new System.Net.Mail.SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSSL
            };

            await smtp.SendMailAsync(mail);
        }

       

        // GET: StockOrder/DeleteMedicationStockOrder/5
        public async Task<IActionResult> DeleteMedicationStockOrder(int? id)
        {
            if (id == null)
                if (id == null) return NotFound();

            var item = await _context.MedicationStockOrder
                .Include(m => m.Medication)
                .Include(m => m.StockOrder)
                .FirstOrDefaultAsync(m => m.MedicationStockOrderId == id);

            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost, ActionName("DeleteMedicationStockOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicationStockOrderDeleteConfirmed(int id)
        {
            var item = await _context.MedicationStockOrder.FindAsync(id);
            if (item != null)
            {
                _context.MedicationStockOrder.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> BulkDeleteMedicationStockOrders(int[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Message"] = "No records selected for deletion.";
                return RedirectToAction("Index");
            }

            var items = _context.MedicationStockOrder.Where(m => selectedIds.Contains(m.MedicationStockOrderId));
            _context.MedicationStockOrder.RemoveRange(items);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{selectedIds.Length} records deleted successfully.";
            return RedirectToAction("Index");
        }


        // GET: MedicationStockOrders/Details/5
        public async Task<IActionResult> MedicationStockOrderDetails(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.MedicationStockOrder
                .Include(m => m.Medication)
                .Include(m => m.StockOrder)
                .FirstOrDefaultAsync(m => m.MedicationStockOrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: MedicationStockOrders/Edit/5
        public async Task<IActionResult> MedicationStockOrderEdit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.MedicationStockOrder.FindAsync(id);
            if (order == null)
                return NotFound();

            ViewData["MedicationId"] = new SelectList(_context.Medication, "MedicationId", "Name", order.MedicationId);
            //ViewData["StockOrderId"] = new SelectList(_context.StockOrder, "StockOrderId", "StockOrderId", order.StockOrderId);
            return View(order);
        }

        // POST: MedicationStockOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicationStockOrderEdit(int id, MedicationStockOrder order)
        {
            if (id != order.MedicationStockOrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MedicationStockOrder.Any(e => e.MedicationStockOrderId == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(IndexStockOrder));
            }

            ViewData["MedicationId"] = new SelectList(_context.Medication, "MedicationId", "Name", order.MedicationId);
            //ViewData["StockOrderId"] = new SelectList(_context.StockOrder, "StockOrderId", "StockOrderId", order.StockOrderId);
            return View(order);
        }

        //// GET: MedicationStockOrders/Delete/5
        //public async Task<IActionResult> DeleteMedicationStockOrder(int? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var order = await _context.MedicationStockOrder
        //        .Include(m => m.Medication)
        //        .Include(m => m.StockOrder)
        //        .FirstOrDefaultAsync(m => m.MedicationStockOrderId == id);

        //    if (order == null)
        //        return NotFound();

        //    return View(order);
        //}

        //// POST: MedicationStockOrders/Delete/5
        //[HttpPost, ActionName("DeleteMedicationStockOrder")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MedicationStockOrderDeleteConfirmed(int id)
        //{
        //    var order = await _context.MedicationStockOrder.FindAsync(id);
        //    if (order != null)
        //    {
        //        _context.MedicationStockOrder.Remove(order);
        //        await _context.SaveChangesAsync();
        //    }
        //    return RedirectToAction(nameof(IndexStockOrder));
        //}


        // GET: StockOrders
        public async Task<IActionResult> IndexStock()
        {
            var stockOrders = _context.StockOrder.Include(s => s.Supplier);
            return View(await stockOrders.ToListAsync());
        }
        public async Task<IActionResult> EditStock(int? id)
        {
            if (id == null)
                return NotFound();

            var stockOrder = await _context.StockOrder.FindAsync(id);
            if (stockOrder == null)
                return NotFound();

            var viewModel = new StockOrderViewModel
            {
                StockOrderId = stockOrder.StockOrderId,
                OrderNumber = stockOrder.OrderNumber,
                //SupplierId = stockOrder.SupplierId,
                OrderDate = stockOrder.OrderDate,
                Status = stockOrder.Status
            };

            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", viewModel.SupplierId);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStock(int id, StockOrderViewModel viewModel)
        {
            if (id != viewModel.StockOrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var stockOrder = await _context.StockOrder.FindAsync(id);
                    if (stockOrder == null)
                        return NotFound();

                    //stockOrder.SupplierId = viewModel.SupplierId;
                    stockOrder.OrderDate = viewModel.OrderDate;
                    stockOrder.Status = viewModel.Status;

                    _context.Update(stockOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.StockOrder.Any(e => e.StockOrderId == viewModel.StockOrderId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(IndexStock));
            }

            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", viewModel.SupplierId);
            return View(viewModel);
        }

        public async Task<IActionResult> DeleteStock(int? id)
        {
            if (id == null)
                return NotFound();

            var stockOrder = await _context.StockOrder
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.StockOrderId == id);

            if (stockOrder == null)
                return NotFound();

            return View(stockOrder);
        }

        [HttpPost, ActionName("DeleteStock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StockDeleteConfirmed(int id)
        {
            var stockOrder = await _context.StockOrder.FindAsync(id);
            if (stockOrder != null)
            {
                _context.StockOrder.Remove(stockOrder);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(IndexStock));
        }
        public async Task<IActionResult> StockDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockOrder = await _context.StockOrder
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.StockOrderId == id);

            if (stockOrder == null)
            {
                return NotFound();
            }

            var viewModel = new StockOrderViewModel
            {
                StockOrderId = stockOrder.StockOrderId,
                OrderNumber = stockOrder.OrderNumber,
                //SupplierId = stockOrder.SupplierId,
                OrderDate = stockOrder.OrderDate,
                Status = stockOrder.Status
            };

            ViewBag.SupplierName = stockOrder.Supplier?.SupplierName;

            return View(viewModel);
        }
        // this is for the notification once the prescription is dispensed

       
    // Called when a prescription is dispensed
   

        private bool MedicationExists(int id)
        {
            return _context.Medication.Any(e => e.MedicationId == id);
        }
        private bool SupplierExists(int id)
        {
            return _context.Supplier.Any(e => e.SupplierId == id);
        }
        private bool PharmacistExists(int id)
        {
            return _context.Pharmacist.Any(e => e.PharmacistId == id);
        }
        private bool DorsageFormExists(int id)
        {
            return _context.DorsageForm.Any(e => e.DorsageFormId == id);
        }
        private bool CustomerAllergyExists(int id)
        {
            return _context.CustomerAllergy.Any(e => e.CustomerAllergyId == id);
        }
        private bool ActiveIngredientsExists(int id)
        {
            return _context.ActiveIngredients.Any(e => e.ActiveIngredientId == id);
        }


    }
}

