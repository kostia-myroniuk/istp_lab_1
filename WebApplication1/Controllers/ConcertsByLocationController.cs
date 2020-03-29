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
    public class ConcertsByLocationController : Controller
    {
        private readonly ConcertsContext _context;

        public ConcertsByLocationController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: Concerts
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                return RedirectToAction("Locations", "Index");
            }
            //if (name == null)
            //{
            //    return RedirectToAction("Artists", "Index");
            //}
            ViewBag.LocationId = id;
            ViewBag.LocationName = name;

            var concertsByLocation = _context.Concerts.Where(c => c.LocationId == id);

            return View(await concertsByLocation.ToListAsync());
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
        public IActionResult Create(int locationId)
        {
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Adress");

            ViewBag.LocationId = locationId;
            ViewBag.LocationName = _context.Locations.Where(l => l.Id == locationId).FirstOrDefault().Name;
            return View();
        }

        // POST: Concerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int locationId, [Bind("Id,Name,Description,Date")] Concerts concerts)
        {
            concerts.LocationId = locationId;
            if (ModelState.IsValid)
            {
                _context.Add(concerts);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "ConcertsByLocation", new { id = locationId, name = _context.Locations.Where(l => l.Id == locationId).FirstOrDefault().Name });
            }
            //ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Adress", concerts.LocationId);
            //return View(concerts);
            return RedirectToAction("Index", "ConcertsByLocation", new { id = locationId, name = _context.Locations.Where(l => l.Id == locationId).FirstOrDefault().Name });
        }

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

        // POST: Concerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concerts = await _context.Concerts.FindAsync(id);
            _context.Concerts.Remove(concerts);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Locations");
        }

        private bool ConcertsExists(int id)
        {
            return _context.Concerts.Any(e => e.Id == id);
        }
    }
}
