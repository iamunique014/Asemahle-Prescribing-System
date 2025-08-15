using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;


namespace YourNamespace.Controllers
{
    public class CustomerProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CustomerProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: CustomerProfile
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var allAllergies = await _context.ActiveIngredients.ToListAsync();
            var selectedAllergyIds = await _context.UserAllergies
                .Where(ua => ua.UserId == user.Id)
                .Select(ua => ua.ActiveIngredientId)
                .ToListAsync();

            var model = new CustomerProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IdentityNumber = user.IdentityNumber,
                Email = user.Email,
                AvailableAllergies = allAllergies.Select(ai => new SelectListItem
                {
                    Value = ai.ActiveIngredientId.ToString(),
                    Text = ai.ActiveIngredientName,
                    Selected = selectedAllergyIds.Contains(ai.ActiveIngredientId)
                }).ToList(),
                SelectedAllergyIds = selectedAllergyIds
            };


            return View(model);
        }

        // POST: CustomerProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CustomerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAllergies(model);
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // Update user basic info
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.IdentityNumber = model.IdentityNumber;
      
            await _userManager.UpdateAsync(user);

            // Update user allergies
            var existingAllergies = _context.UserAllergies.Where(ua => ua.UserId == user.Id);
            _context.UserAllergies.RemoveRange(existingAllergies);

            if (model.SelectedAllergyIds != null && model.SelectedAllergyIds.Any())
            {
                var newAllergies = model.SelectedAllergyIds
                    .Distinct() // prevent duplicates
                    .Select(id => new UserAllergy
                    {
                        UserId = user.Id,
                        ActiveIngredientId = id
                    });

                _context.UserAllergies.AddRange(newAllergies);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Helper method to populate AvailableAllergies in case of validation error
        private async Task PopulateAllergies(CustomerProfileViewModel model)
        {
            var allAllergies = await _context.ActiveIngredients.ToListAsync();
            model.AvailableAllergies = allAllergies
                .Select(ai => new SelectListItem
                {
                    Value = ai.ActiveIngredientId.ToString(),
                    Text = ai.ActiveIngredientName,
                    Selected = model.SelectedAllergyIds != null && model.SelectedAllergyIds.Contains(ai.ActiveIngredientId)
                })
                .ToList();
        }
    }
}
