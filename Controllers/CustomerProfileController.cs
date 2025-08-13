using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.AunthViewModels;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using System.Security.Claims;

//[Authorize(Roles = "Customer")]
public class CustomerProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /CustomerProfile
    public async Task<IActionResult> CustomerProfil()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null) return NotFound();

        // Load allergies for this user (IDs)
        var selectedIds = await _context.CustomerAllergies
            .Where(a => a.CustomerId == user.Id)
            .Select(a => a.ActiveIngredientId)
            .ToListAsync();

        var model = new CustomerProfileViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            SelectedAllergyIds = selectedIds,
            AvailableIngredients = await _context.ActiveIngredients
                                                .OrderBy(i => i.ActiveIngredientName)
                                                .ToListAsync()
        };

        return View(model);
    }

    // POST: /CustomerProfile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CustomerProfil(CustomerProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // repopulate list on validation error
            model.AvailableIngredients = await _context.ActiveIngredients.OrderBy(i => i.ActiveIngredientName).ToListAsync();
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null) return NotFound();

        // Update basic fields
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        // If you want to change Username to match email: user.UserName = model.Email;

        await _userManager.UpdateAsync(user);

        // Update allergies: simple approach = remove all then add selected
        var existing = _context.CustomerAllergies.Where(a => a.CustomerId == user.Id);
        _context.CustomerAllergies.RemoveRange(existing);

        var toAdd = model.SelectedAllergyIds.Distinct().Select(id => new CustomerAllergies
        {
            CustomerId = user.Id,
            ActiveIngredientId = id
        });

        await _context.CustomerAllergies.AddRangeAsync(toAdd);
        await _context.SaveChangesAsync();

        TempData["ProfileSaved"] = "Profile updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}
