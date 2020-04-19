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
    [Route("Orders/[action]")]
    public class TicketsBySectorController : Controller
    {
        private readonly ConcertsContext _context;

        public TicketsBySectorController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: TicketsBySector
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Sectors");
            }

            ViewBag.SectorId = id;
            ViewBag.SectorName = name;

            var concertsContext = _context.Tickets.Where(t => t.SectorId == id).Include(t => t.Sector).Include(t => t.Status).Include(t => t.Client).Include(t => t.Sector).ThenInclude(t => t.Concert);
            return View(await concertsContext.ToListAsync());
        }

        // GET: TicketsBySector/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.Sector)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // GET: TicketsBySector/Create
        public IActionResult Create(int sectorId)
        {
            ViewBag.SectorId = sectorId;
            ViewBag.SectorName = _context.Sectors.Where(s => s.Id == sectorId).FirstOrDefault().Name;
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            //ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
            return View();
        }

        // POST: TicketsBySector/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int sectorId, [Bind("Id,ClientId,StatusId")] Tickets tickets)
        {
            tickets.SectorId = sectorId;
            if (ModelState.IsValid)
            {
                _context.Add(tickets);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("index", "TicketsBySector", new { id = sectorId, name = _context.Sectors.Where(s => s.Id == sectorId).FirstOrDefault().Name });
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", tickets.ClientId);
            //ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", tickets.SectorId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", tickets.StatusId);
            //return View(tickets);
            return RedirectToAction("index", "TicketsBySector", new { id = sectorId, name = _context.Sectors.Where(s => s.Id == sectorId).FirstOrDefault().Name });
        }

        // GET: TicketsBySector/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets.FindAsync(id);
            if (tickets == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", tickets.ClientId);
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", tickets.SectorId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", tickets.StatusId);
            return View(tickets);
        }

        // POST: TicketsBySector/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SectorId,ClientId,StatusId")] Tickets tickets)
        {
            if (id != tickets.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tickets);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketsExists(tickets.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Sectors");
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", tickets.ClientId);
            ViewData["SectorId"] = new SelectList(_context.Sectors, "Id", "Name", tickets.SectorId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", tickets.StatusId);
            return View(tickets);
        }

        // GET: TicketsBySector/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tickets = await _context.Tickets
                .Include(t => t.Client)
                .Include(t => t.Sector)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tickets == null)
            {
                return NotFound();
            }

            return View(tickets);
        }

        // POST: TicketsBySector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tickets = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(tickets);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Sectors");
        }

        private bool TicketsExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
