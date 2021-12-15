using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using yuricafe2._0.Data;
using yuricafe2._0.Models;

namespace yuricafe2._0.Controllers
{
    public class coffeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public coffeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: coffees
        public async Task<IActionResult> Index()
        {
            return View(await _context.coffee.ToListAsync());
        }

        // GET: coffees/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // GET: coffees/ShowSearchResults
        [Authorize]
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index",await _context.coffee.Where(j =>j.coffeename.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: coffees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.coffee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // GET: coffees/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: coffees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,coffeename,coffeeprice")] coffee coffee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coffee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coffee);
        }

        // GET: coffees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.coffee.FindAsync(id);
            if (coffee == null)
            {
                return NotFound();
            }
            return View(coffee);
        }

        // POST: coffees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,coffeename,coffeeprice")] coffee coffee)
        {
            if (id != coffee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coffee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!coffeeExists(coffee.Id))
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
            return View(coffee);
        }

        // GET: coffees/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coffee = await _context.coffee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coffee == null)
            {
                return NotFound();
            }

            return View(coffee);
        }

        // POST: coffees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coffee = await _context.coffee.FindAsync(id);
            _context.coffee.Remove(coffee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool coffeeExists(int id)
        {
            return _context.coffee.Any(e => e.Id == id);
        }
    }
}
