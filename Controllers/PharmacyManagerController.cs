using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using System.Linq;
using OfficeOpenXml;
using PrescribingSystem.Models.ViewModels;

namespace PrescribingSystem.Controllers
{
    public class PharmacyManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PharmacyManagerController(ApplicationDbContext context)
        {
            _context = context;
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
        //End of doctors //

        /// <summary>
        /// ////////
        /// </summary>
        /// <returns></returns>

        // GET: CustomerAllergy
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
        /// <summary>
        /// 
        /// </summary>
       
        /// <returns></returns>
        /// 
        // GET: Pharmacist
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
        /// <summary>
        /// 

        /// <returns></returns>
        /// 
        // GET: Pharmacy/Create
        // GET: Pharmacy
        public async Task<IActionResult> IndexPharmacy()
        {
            // Include the related Pharmacist data along with the Pharmacy
            var pharmacies = await _context.Pharmacy
                                            .Include(p => p.Pharmacist) // Eager load Pharmacist data
                                            .ToListAsync(); // Retrieve the list of pharmacies

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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        // GET: Medication/Create
        public IActionResult AddMedication()
        {
            // Populate dropdowns using ViewBag
            ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName");
            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName");
            // Populate the Dosage Form dropdown with values from the database
            //ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName");

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
                    Status = model.Status
                };

                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexMedication");
            }

            // Repopulate ViewBag dropdowns on validation error
            ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName", model.DorsageFormId);
            ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", model.SupplierId);

            return View(model);
            // Repopulate dropdowns if validation fails
            //model.DosageForms = _context.DorsageForm
            //    .Select(d => new SelectListItem
            //    {
            //        Value = d.DorsageFormId.ToString(),
            //        Text = d.DorsageFormName
            //    }).ToList();

            //model.Suppliers = _context.Supplier
            //    .Select(s => new SelectListItem
            //    {
            //        Value = s.SupplierId.ToString(),
            //        Text = s.SupplierName
            //    }).ToList();

            //return View(model);
            //if (ModelState.IsValid)
            //{
            //    // Add the new medication to the database
            //    _context.Add(medication);
            //    await _context.SaveChangesAsync();

            //    // Redirect to the list of medications
            //    return RedirectToAction("IndexMedication");
            //}

            //// Repopulate dropdown lists in case of a validation failure
            //ViewBag.DosageFormId = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName", medication.DorsageFormId);
            //ViewBag.SupplierId = new SelectList(_context.Supplier, "SupplierId", "SupplierName", medication.SupplierId);

            //return View(medication);

        }

        // GET: Medication/Index
        public async Task<IActionResult> IndexMedication()
        {
            //return View(await _context.Medication.ToListAsync());
            var medications = await _context.Medication
        .Include(m => m.DorsageForm)
        .Include(m => m.Supplier)
        .ToListAsync();

            return View(medications);
        }

        // GET: Medication/Edit/5
        public async Task<IActionResult> EditMedication(int? id)
        {
            if (id == null) return NotFound();

            var medication = await _context.Medication.FindAsync(id);
            if (medication == null) return NotFound();

            ViewData["DorsageFormId"] = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName", medication.DorsageFormId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName", medication.SupplierId);
            return View(medication);
        }


        // POST: Medication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedication(int id, Medication medication)
        {
            if (id != medication.MedicationId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DorsageFormId"] = new SelectList(_context.DorsageForm, "DorsageFormId", "DorsageFormName", medication.DorsageFormId);
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "SupplierName", medication.SupplierId);
            return View(medication);
        }

        // GET: Medication/Details/5
        public async Task<IActionResult> MedicationDetails(int? id)
        {
            if (id == null) return NotFound();

            var medication = await _context.Medication
                .Include(m => m.DorsageFormId)
                .Include(m => m.SupplierId)
                .FirstOrDefaultAsync(m => m.MedicationId == id);

            if (medication == null) return NotFound();

            return View(medication);
        }

        // GET: Medication/Delete/5
        public async Task<IActionResult> DeleteMedication(int? id)
        {
            if (id == null) return NotFound();

            var medication = await _context.Medication
                .Include(m => m.DorsageFormId)
                .Include(m => m.SupplierId)
                .FirstOrDefaultAsync(m => m.MedicationId == id);

            if (medication == null) return NotFound();

            return View(medication);
        }

        // POST: Medication/Delete/5
        [HttpPost, ActionName("DeleteMedication")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MedicationDeleteConfirmed(int id)
        {
            var medication = await _context.Medication.FindAsync(id);
            _context.Medication.Remove(medication);
            await _context.SaveChangesAsync();
            return RedirectToAction("IndexMedication");
        }
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

