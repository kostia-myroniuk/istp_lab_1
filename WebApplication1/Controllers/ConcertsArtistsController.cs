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
    public class ConcertsArtistsController : Controller
    {
        private readonly ConcertsContext _context;

        public ConcertsArtistsController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: ConcertsArtists
        public async Task<IActionResult> Index()
        {
            var concertsContext = _context.ConcertsArtists.Include(c => c.Artist).Include(c => c.Concert);
            return View(await concertsContext.ToListAsync());
        }

        // GET: ConcertsArtists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertsArtists = await _context.ConcertsArtists
                .Include(c => c.Artist)
                .Include(c => c.Concert)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concertsArtists == null)
            {
                return NotFound();
            }

            return View(concertsArtists);
        }

        // GET: ConcertsArtists/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name");
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Id");
            return View();
        }

        // POST: ConcertsArtists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ConcertId,ArtistId")] ConcertsArtists concertsArtists)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concertsArtists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", concertsArtists.ArtistId);
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Id", concertsArtists.ConcertId);
            return View(concertsArtists);
        }

        // GET: ConcertsArtists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertsArtists = await _context.ConcertsArtists.FindAsync(id);
            if (concertsArtists == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", concertsArtists.ArtistId);
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Id", concertsArtists.ConcertId);
            return View(concertsArtists);
        }

        // POST: ConcertsArtists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ConcertId,ArtistId")] ConcertsArtists concertsArtists)
        {
            if (id != concertsArtists.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concertsArtists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertsArtistsExists(concertsArtists.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", concertsArtists.ArtistId);
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Id", concertsArtists.ConcertId);
            return View(concertsArtists);
        }

        // GET: ConcertsArtists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concertsArtists = await _context.ConcertsArtists
                .Include(c => c.Artist)
                .Include(c => c.Concert)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (concertsArtists == null)
            {
                return NotFound();
            }

            return View(concertsArtists);
        }

        // POST: ConcertsArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concertsArtists = await _context.ConcertsArtists.FindAsync(id);
            _context.ConcertsArtists.Remove(concertsArtists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConcertsArtistsExists(int id)
        {
            return _context.ConcertsArtists.Any(e => e.Id == id);
        }
    }
}
