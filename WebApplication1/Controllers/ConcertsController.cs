using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1;

namespace WebApplication1.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly ConcertsContext _context;

        public ConcertsController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: Concerts
        public async Task<IActionResult> Index()
        {
            var concertsContext = _context.Concerts.Include(c => c.Location).ThenInclude(l => l.City).Include(c => c.ConcertsArtists).ThenInclude(ca => ca.Artist);
            return View(await concertsContext.ToListAsync());
        }

        // GET: Concerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concerts = await _context.Concerts
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concerts == null)
            {
                return NotFound();
            }

            //return View(concerts);
            return RedirectToAction("Index", "ArtistsByConcerts", new { id = concerts.Id, name = concerts.Name });
        }

        // GET: Concerts/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: Concerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Date,LocationId")] Concerts concerts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concerts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", concerts.LocationId);
            return View(concerts);
        }

        [Authorize(Roles = "admin, organizer")]
        // GET: Concerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concerts = await _context.Concerts.FindAsync(id);
            if (concerts == null)
            {
                return NotFound();
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", concerts.LocationId);
            return View(concerts);
        }

        [Authorize(Roles = "admin, organizer")]
        // POST: Concerts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Date,LocationId")] Concerts concerts)
        {
            if (id != concerts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concerts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertsExists(concerts.Id))
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
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", concerts.LocationId);
            return View(concerts);
        }

        [Authorize(Roles = "admin, organizer")]
        // GET: Concerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concerts = await _context.Concerts
                .Include(c => c.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concerts == null)
            {
                return NotFound();
            }

            return View(concerts);
        }

        [Authorize(Roles = "admin, organizer")]
        // POST: Concerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concerts = await _context.Concerts.FindAsync(id);
            _context.Concerts.Remove(concerts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertsExists(int id)
        {
            return _context.Concerts.Any(e => e.Id == id);
        }
    }
}
