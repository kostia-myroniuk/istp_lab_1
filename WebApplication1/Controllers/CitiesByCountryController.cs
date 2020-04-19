using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1;

namespace WebApplication1.Controllers
{
    [Route("Cities/[action]")]
    public class CitiesByCountryController : Controller
    {
        private readonly ConcertsContext _context;

        public CitiesByCountryController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: CitiesByCountry
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                return RedirectToAction("Countries", "Index");
            }
            ViewBag.CountryId = id;
            ViewBag.CountryName = name;

            var citiesByCountry = _context.Cities.Where(c => c.CountryId == id).OrderBy(c => c.Name);
            return View(await citiesByCountry.ToListAsync());
        }

        // GET: CitiesByCountry/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cities = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cities == null)
            {
                return NotFound();
            }

            return View(cities);
        }

        // GET: CitiesByCountry/Create
        public IActionResult Create(int countryId)
        {
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewBag.CountryId = countryId;
            ViewBag.CountryName = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name;
            return View();
        }

        // POST: CitiesByCountry/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int countryId, [Bind("Id,Name")] Cities cities)
        {
            cities.CountryId = countryId;
            if (ModelState.IsValid && !_context.Cities.Any(c => c.CountryId == countryId && c.Name == cities.Name))
            {
                _context.Add(cities);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "CitiesByCountry", new { id = countryId, name = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name }); 
            }
            //ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", cities.CountryId);
            //return View(cities);
            return RedirectToAction("Index", "CitiesByCountry", new { id = countryId, name = _context.Countries.Where(c => c.Id == countryId).FirstOrDefault().Name });
        }

        // GET: CitiesByCountry/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cities = await _context.Cities.FindAsync(id);
            if (cities == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", cities.CountryId);
            return View(cities);
        }

        // POST: CitiesByCountry/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryId")] Cities cities)
        {
            if (id != cities.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitiesExists(cities.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Countries");
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", cities.CountryId);
            return View(cities);
        }

        // GET: CitiesByCountry/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cities = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cities == null)
            {
                return NotFound();
            }

            return View(cities);
        }

        // POST: CitiesByCountry/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cities = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(cities);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Countries");
        }

        private bool CitiesExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
