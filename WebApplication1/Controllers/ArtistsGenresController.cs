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
    public class ArtistsGenresController : Controller
    {
        private readonly ConcertsContext _context;

        public ArtistsGenresController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: ArtistsGenres
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Artists");
            }

            var concertsContext = _context.ArtistsGenres.Where(ag => ag.ArtistId == id).Include(a => a.Artist).Include(a => a.Genre);
            ViewBag.ArtistId = id;
            ViewBag.ArtistName = _context.Artists.Where(a => a.Id == id).FirstOrDefault().Name;
            return View(await concertsContext.ToListAsync());
        }

        // GET: ArtistsGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistsGenres = await _context.ArtistsGenres
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistsGenres == null)
            {
                return NotFound();
            }

            return View(artistsGenres);
        }

        // GET: ArtistsGenres/Create
        public IActionResult Create(int artistId)
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.ArtistId = artistId;
            ViewBag.ArtistName = _context.Artists.Where(a => a.Id == artistId).FirstOrDefault().Name;
            return View();
        }

        // POST: ArtistsGenres/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int artistId, [Bind("Id,GenreId")] ArtistsGenres artistsGenres)
        {
            artistsGenres.ArtistId = artistId;
            if (ModelState.IsValid && !_context.ArtistsGenres.Any(ag => ag.GenreId == artistsGenres.GenreId && ag.ArtistId == artistId))
            {
                _context.Add(artistsGenres);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "ArtistsGenres", new { id = artistId, name = _context.Artists.Where(a => a.Id == artistId).FirstOrDefault().Name });
            }
            //ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", artistsGenres.ArtistId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", artistsGenres.GenreId);
            return RedirectToAction("Index", "ArtistsGenres", new { id = artistId, name = _context.Artists.Where(a => a.Id == artistId).FirstOrDefault().Name });
            //return View(artistsGenres);
        }

        // GET: ArtistsGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistsGenres = await _context.ArtistsGenres.FindAsync(id);
            if (artistsGenres == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", artistsGenres.ArtistId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", artistsGenres.GenreId);
            return View(artistsGenres);
        }

        // POST: ArtistsGenres/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistId,GenreId")] ArtistsGenres artistsGenres)
        {
            if (id != artistsGenres.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistsGenres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistsGenresExists(artistsGenres.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", artistsGenres.ArtistId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", artistsGenres.GenreId);
            return View(artistsGenres);
        }

        // GET: ArtistsGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistsGenres = await _context.ArtistsGenres
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artistsGenres == null)
            {
                return NotFound();
            }

            return View(artistsGenres);
        }

        // POST: ArtistsGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int artistId)
        {
            var artistsGenres = await _context.ArtistsGenres.FindAsync(id);
            _context.ArtistsGenres.Remove(artistsGenres);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = artistId });
        }

        private bool ArtistsGenresExists(int id)
        {
            return _context.ArtistsGenres.Any(e => e.Id == id);
        }
    }
}
