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
    public class ArtistsByGenreController : Controller
    {
        private readonly ConcertsContext _context;

        public ArtistsByGenreController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: ArtistsByGenre
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Genres");
            }
            ViewBag.GenreId = id;
            ViewBag.GenreName = name;
            var artistsByGenre = _context.ArtistsGenres.Where(ag => ag.GenreId == id).Include(ag => ag.Artist);

            return View(await artistsByGenre.ToListAsync());
        }

        // GET: ArtistsByGenre/Details/5
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

        // GET: ArtistsByGenre/Create
        public IActionResult Create(int genreId)
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name");
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewBag.GenreId = genreId;
            ViewBag.GenreName = _context.Genres.Where(g => g.Id == genreId).FirstOrDefault().Name;
            return View();
        }

        // POST: ArtistsByGenre/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int genreId, [Bind("Id,ArtistId")] ArtistsGenres artistsGenres)
        {
            artistsGenres.GenreId = genreId;
            if (ModelState.IsValid)
            {
                _context.Add(artistsGenres);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "ArtistsByGenre", new { id = genreId, name = _context.Genres.Where(g => g.Id == genreId).FirstOrDefault().Name });
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Name", artistsGenres.ArtistId);
            //ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", artistsGenres.GenreId);
            //return View(artistsGenres);
            return RedirectToAction("Index", "ArtistsByGenre", new { id = genreId, name = _context.Genres.Where(g => g.Id == genreId).FirstOrDefault().Name });
        }

        // GET: ArtistsByGenre/Edit/5
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

        // POST: ArtistsByGenre/Edit/5
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

        // GET: ArtistsByGenre/Delete/5
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

        // POST: ArtistsByGenre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artistsGenres = await _context.ArtistsGenres.FindAsync(id);
            _context.ArtistsGenres.Remove(artistsGenres);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistsGenresExists(int id)
        {
            return _context.ArtistsGenres.Any(e => e.Id == id);
        }
    }
}
