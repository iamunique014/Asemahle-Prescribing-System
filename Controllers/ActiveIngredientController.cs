using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrescribingSystem.Data;
using PrescribingSystem.Models;

namespace PrescribingSystem.Controllers
{
    public class ActiveIngredientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActiveIngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ActiveIngredient
        public async Task<IActionResult> Index()
        {
            return View(await _context.ActiveIngredients.ToListAsync());
        }

        // GET: ActiveIngredient/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ActiveIngredient/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActiveIngredient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActiveIngredientId,ActiveIngredientName")] ActiveIngredients activeIngredients)
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
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("ActiveIngredientId,ActiveIngredientName")] ActiveIngredients activeIngredients)
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
        public async Task<IActionResult> Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
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

        private bool ActiveIngredientsExists(int id)
        {
            return _context.ActiveIngredients.Any(e => e.ActiveIngredientId == id);
        }
    }
}
