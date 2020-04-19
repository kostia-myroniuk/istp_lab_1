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
    [Route("Lineup/[action]")]
    public class ArtistsByConcertsController : Controller
    {
        private readonly ConcertsContext _context;

        public ArtistsByConcertsController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: ArtistsByConcerts
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Concerts");
            }
            ViewBag.ConcertId = id;
            ViewBag.ConcertName = name;
            var concertsContext = _context.ConcertsArtists.Where(c => c.ConcertId == id).Include(c => c.Artist);
            //concertsContext.OrderBy(c => c.Artist.Name);
            return View(await concertsContext.ToListAsync());
        }

        // GET: ArtistsByConcerts/Details/5
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

        // GET: ArtistsByConcerts/Create
        public IActionResult Create(int concertId)
        {
            ViewBag.ConcertId = concertId;
            ViewBag.ConcertName = _context.Concerts.Where(c => c.Id == concertId).FirstOrDefault().Name;
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name");
            //ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Id");
            return View();
        }

        // POST: ArtistsByConcerts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int concertId, [Bind("Id,ArtistId")] ConcertsArtists concertsArtists)
        {
            concertsArtists.ConcertId = concertId;
            if (ModelState.IsValid && !_context.ConcertsArtists.Any(ca => ca.ConcertId == concertId && ca.ArtistId == concertsArtists.ArtistId))
            {
                _context.Add(concertsArtists);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "ArtistsByConcerts", new { id = concertId, name = _context.Concerts.Where(c => c.Id == concertId).FirstOrDefault().Name });
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", concertsArtists.ArtistId);
            //ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Id", concertsArtists.ConcertId);
            return RedirectToAction("Index", "ArtistsByConcerts", new { id = concertId, name = _context.Concerts.Where(c => c.Id == concertId).FirstOrDefault().Name });
            //return View(concertsArtists);
        }

        // GET: ArtistsByConcerts/Edit/5
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
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Name", concertsArtists.ConcertId);
            return View(concertsArtists);
        }

        // POST: ArtistsByConcerts/Edit/5
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
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Name", concertsArtists.ConcertId);
            return View(concertsArtists);
        }

        // GET: ArtistsByConcerts/Delete/5
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

        // POST: ArtistsByConcerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int concertId)
        {
            var concertsArtists = await _context.ConcertsArtists.FindAsync(id);
            _context.ConcertsArtists.Remove(concertsArtists);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = concertId, name = _context.Concerts.Where(c => c.Id == concertId).FirstOrDefault().Name });
        }

        private bool ConcertsArtistsExists(int id)
        {
            return _context.ConcertsArtists.Any(e => e.Id == id);
        }
    }
}
