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
    public class SectorsController : Controller
    {
        private readonly ConcertsContext _context;

        public SectorsController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: Sectors
        //public async Task<IActionResult> Index()
        //{
        //    var concertsContext = _context.Sectors.Include(s => s.Concert).OrderBy(cc => cc.Concert.Name).ThenBy(cc => cc.Price);
        //    return View(await concertsContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                //return RedirectToAction("Index", "Concerts");
                var concertsContext = _context.Sectors.Include(s => s.Concert).OrderBy(cc => cc.Concert.Name).ThenBy(cc => cc.Price);
                return View(await concertsContext.ToListAsync());
            }
            else
            {
                var concertsContext = _context.Sectors.Where(s => s.ConcertId == id).Include(s => s.Concert).OrderBy(cc => cc.Concert.Name).ThenBy(cc => cc.Price);
                return View(await concertsContext.ToListAsync());
            }
        }

        // GET: Sectors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectors = await _context.Sectors
                .Include(s => s.Concert)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectors == null)
            {
                return NotFound();
            }

            //return View(sectors);
            return RedirectToAction("Index", "TicketsBySector", new { id = sectors.Id, name = sectors.Name });
        }

        // GET: Sectors/Create
        public IActionResult Create()
        {
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Name");
            return View();
        }

        // POST: Sectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ConcertId,Price")] Sectors sectors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sectors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Name", sectors.ConcertId);
            return View(sectors);
        }

        // GET: Sectors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectors = await _context.Sectors.FindAsync(id);
            if (sectors == null)
            {
                return NotFound();
            }
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Name", sectors.ConcertId);
            return View(sectors);
        }

        // POST: Sectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ConcertId,Price")] Sectors sectors)
        {
            if (id != sectors.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sectors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectorsExists(sectors.Id))
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
            ViewData["ConcertId"] = new SelectList(_context.Concerts, "Id", "Name", sectors.ConcertId);
            return View(sectors);
        }

        // GET: Sectors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sectors = await _context.Sectors
                .Include(s => s.Concert)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sectors == null)
            {
                return NotFound();
            }

            return View(sectors);
        }

        // POST: Sectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sectors = await _context.Sectors.FindAsync(id);
            _context.Sectors.Remove(sectors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectorsExists(int id)
        {
            return _context.Sectors.Any(e => e.Id == id);
        }
    }
}
