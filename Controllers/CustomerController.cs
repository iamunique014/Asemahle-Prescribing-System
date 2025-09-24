using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;
using PrescribingSystem.Models.ViewModels;

namespace PrescribingSystem.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult AddCustomerAllergies()
        {

            var activeIngrediets = _context.ActiveIngredients
                .Select(ai => new ActiveIngredientViewModel
                {
                    id = ai.ActiveIngredientId,
                    Name = ai.ActiveIngredientName

                }).ToList();

            return View(activeIngrediets);      
        }
    }
}
