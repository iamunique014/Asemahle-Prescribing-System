using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}
