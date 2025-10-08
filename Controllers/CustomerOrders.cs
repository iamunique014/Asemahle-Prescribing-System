using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;
using System.Security.Claims;

namespace PrescribingSystem.Controllers
{
    public class CustomerOrders : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerOrders(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OrderPrescribedMedications( )
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                // User not logged in redirect to login
                return RedirectToPage("/Account/Login");
            }

            var prescribedMedication = await _context.MedicationItems
                .Include(mi => mi.Medication)
                .Include(mi => mi.Prescription)
                .Where(mi => mi.Prescription.CustomerId == customerId && mi.RemainingRepeats > 0)
                .Select(mi => new AvailableMedicationViewModel
                {
                    MedicationItemId = mi.MedicationItemId,
                    MedicationName = mi.Medication.Name,
                    DoctorName = mi.Prescription.DoctorName,
                    Quantity = mi.Quantity,
                    RemainingRepeats = mi.RemainingRepeats
                })
                .ToListAsync();

            return View(prescribedMedication);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCustomerOrder(List<int> selectedMedicationItemIds)
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                // User not logged in redirect to login
                return RedirectToPage("/Account/Login");
            }

            if (selectedMedicationItemIds == null || !selectedMedicationItemIds.Any())
                return RedirectToAction("Create");

            var items = await _context.MedicationItems
                .Include(mi => mi.Medication)
                .Where(mi => selectedMedicationItemIds.Contains(mi.MedicationItemId))
                .ToListAsync();

            var order = new PrescriptionOrders
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                OrderStatus = OrderStatus.Pending,
                TotalCost = items.Sum(i => i.Quantity * i.Medication.CurrentSalesPrice)
            };

            _context.PrescriptionOrders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                _context.OrderItems.Add(new OrderItem
                {
                    OrderId = order.PrescriptionOrdersId,
                    MedicationItemId = item.MedicationItemId,
                    QuantityOrdered = item.Quantity,
                    UnitPrice = item.Medication.CurrentSalesPrice,
                    LineTotal = item.Quantity * item.Medication.CurrentSalesPrice
                });

                item.RemainingRepeats--;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("MyOrders");
        }
        public async Task<IActionResult> MyOrders()
        {
            string customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(customerId))
            {
                // User not logged in redirect to login
                return RedirectToPage("/Account/Login");
            }

            var orders = await _context.PrescriptionOrders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MedicationItem)
                .ThenInclude(mi => mi.Medication)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
    }
}
