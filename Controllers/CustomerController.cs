using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;
using System.Security.Claims;

namespace PrescribingSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult CustomerCare()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new EditCustomerProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                CellphoneNumber = user.CellphoneNumber,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditCustomerProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.CellphoneNumber = model.CellphoneNumber;

            await _userManager.UpdateAsync(user);

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(EditProfile));
        }

        [HttpGet]
        public IActionResult AddCustomerAllergies(string id)
        {
            var viewmodel = new AddCustomerAllergiesViewModel
            {
                UserId = id,
                ActiveIngredients = _context.ActiveIngredients
                    .Select(ai => new ActiveIngredientViewModel
                    {
                        id = ai.ActiveIngredientId,
                        Name = ai.ActiveIngredientName
                    }).ToList()
            };

            return View(viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerAllergies(AddCustomerAllergiesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound("Customer not found");

            foreach (var allergyId in model.SelectedAllergies)
            {
                _context.UserAllergies.Add(new UserAllergy
                {
                    UserId = model.UserId,
                    ActiveIngredientId = allergyId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Landing", "Landing"); // or customer dashboard
        }

        [HttpGet]
        public async Task<IActionResult> ManageAllergies()
        {
            var user = await _userManager.GetUserAsync(User);
            var allIngredients = await _context.ActiveIngredients.ToListAsync();
            var userAllergies = await _context.UserAllergies
                .Where(a => a.UserId == user.Id)
                .Select(a => a.ActiveIngredientId)
                .ToListAsync();

            var model = new ManageAllergiesViewModel
            {
                SelectedAllergyIds = userAllergies,
                AvailableAllergies = allIngredients.Select(ai => new SelectListItem
                {
                    Value = ai.ActiveIngredientId.ToString(),
                    Text = ai.ActiveIngredientName,
                    Selected = userAllergies.Contains(ai.ActiveIngredientId)
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageAllergies(ManageAllergiesViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var existing = _context.UserAllergies.Where(a => a.UserId == user.Id);
            _context.UserAllergies.RemoveRange(existing);

            foreach (var id in model.SelectedAllergyIds)
            {
                _context.UserAllergies.Add(new UserAllergy { UserId = user.Id, ActiveIngredientId = id });
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Allergies updated successfully.";
            return RedirectToAction(nameof(ManageAllergies));
        }
    }
}
