using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using WebApplication1;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WebApplication1.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly ConcertsContext _context;

        public ArtistsController(ConcertsContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            // var concertsContext = _context.Artists.Include(a => a.Country).Include(a => a.ArtistsGenres).ThenInclude(ag => ag.);

            var concertsContext = _context.Artists.Include(a => a.Country).Include(a => a.ArtistsGenres).ThenInclude(ag => ag.Genre);

            return View(await concertsContext.ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artists == null)
            {
                return NotFound();
            }

            //return View(artists);
            return RedirectToAction("Index", "ConcertsByLocation", new { id = artists.Id, name = artists.Name });
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CountryId")] Artists artists)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", artists.CountryId);
            return View(artists);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists.FindAsync(id);
            if (artists == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", artists.CountryId);
            return View(artists);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CountryId")] Artists artists)
        {
            if (id != artists.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistsExists(artists.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", artists.CountryId);
            return View(artists);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists
                .Include(a => a.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artists == null)
            {
                return NotFound();
            }

            return View(artists);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artists = await _context.Artists.FindAsync(id);
            _context.Artists.Remove(artists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistsExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workbook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workbook.Worksheets)
                            {
                                Countries country;
                                var c1 = (from c2 in _context.Countries
                                          where c2.Name.Contains(worksheet.Name)
                                          select c2).ToList();

                                if (c1.Count > 0)
                                {
                                    country = c1[0];
                                }
                                else
                                {
                                    country = new Countries();
                                    country.Name = worksheet.Name;
                                    _context.Countries.Add(country);
                                }

                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Artists artist = new Artists();
                                        artist.Name = row.Cell(1).Value.ToString();
                                        artist.Description = row.Cell(6).Value.ToString();
                                        artist.Country = country;
                                        _context.Artists.Add(artist);
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Genres genre = null;
                                                
                                                foreach (Genres g in _context.Genres)
                                                {
                                                    string nameNoSpaces = g.Name;
                                                    nameNoSpaces.Replace(" ", "");
                                                    string cellName = row.Cell(i).Value.ToString();
                                                    cellName.Replace(" ", "");
                                                    if (nameNoSpaces == cellName)
                                                    {
                                                        genre = g;
                                                        break;
                                                    }
                                                }

                                                if (genre == null)
                                                {
                                                    genre = new Genres();
                                                    genre.Name = row.Cell(i).Value.ToString();
                                                    _context.Add(genre);
                                                }

                                                ArtistsGenres ag = new ArtistsGenres();
                                                ag.Artist = artist;
                                                ag.Genre = genre;
                                                _context.ArtistsGenres.Add(ag);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        return RedirectToAction("Error", "Home");
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult MakeReport()
        {          
            using (var stream = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());

                    {
                        ParagraphProperties pp = new ParagraphProperties();
                        Justification j1 = new Justification() { Val = JustificationValues.Center };
                        pp.Append(j1);
                        Paragraph title = body.AppendChild(new Paragraph(pp));
                        title.Append(new Run(new Text("Звіт")));
                        Paragraph par = body.AppendChild(new Paragraph());
                        par.Append(new Run(new Text($"Звіт створено: {DateTime.UtcNow.ToLongDateString()} {DateTime.UtcNow.ToShortTimeString()} (UTC+0)")));
                        par = body.AppendChild(new Paragraph());
                        //par.Append(new Run(new Break()));
                    }

                    {
                        List<Tuple<string, string>> amounts = new List<Tuple<string, string>>();
                        amounts.Add(new Tuple<string, string>("Виконавці", $"{_context.Artists.Count()}"));
                        amounts.Add(new Tuple<string, string>("Виконавці / Жанри", $"{_context.ArtistsGenres.Count()}"));
                        amounts.Add(new Tuple<string, string>("Міста", $"{_context.Cities.Count()}"));
                        amounts.Add(new Tuple<string, string>("Клієнти", $"{_context.Clients.Count()}"));
                        amounts.Add(new Tuple<string, string>("Концерти", $"{_context.Concerts.Count()}"));
                        amounts.Add(new Tuple<string, string>("Концерти / Виконавці", $"{_context.ConcertsArtists.Count()}"));
                        amounts.Add(new Tuple<string, string>("Країни", $"{_context.Countries.Count()}"));
                        amounts.Add(new Tuple<string, string>("Жанри", $"{_context.Genres.Count()}"));
                        amounts.Add(new Tuple<string, string>("Місця", $"{_context.Locations.Count()}"));
                        amounts.Add(new Tuple<string, string>("Сектори", $"{_context.Sectors.Count()}"));
                        amounts.Add(new Tuple<string, string>("Квитки", $"{_context.Tickets.Count()}"));

                        Paragraph t = body.AppendChild(new Paragraph());
                        t.Append(new Run(new Text("Кількість записів в БД:")));

                        foreach (Tuple<string,string> amount in amounts)
                        {
                            Paragraph par = body.AppendChild(new Paragraph());
                            par.ParagraphProperties = new ParagraphProperties(new Tabs(new TabStop() { Val = TabStopValues.Right, Position = 9000, Leader = TabStopLeaderCharValues.Dot }));
                            par.Append(new Run(new Text(amount.Item1)));
                            par.Append(new Run(new TabChar()));
                            par.Append(new Run(new Text(amount.Item2)));
                        }
                    }

                    {
                        Paragraph par = body.AppendChild(new Paragraph());
                        decimal sum = 0;

                        var t = _context.Tickets.Include(t => t.Sector);

                        foreach (Tickets ticket in t)
                        {
                            sum += ticket.Sector.Price;
                        }

                        par = body.AppendChild(new Paragraph());
                        par.Append(new Run(new Text($"Загальна вартість квитків - {sum.ToString("F2")} грн")));
                        par = body.AppendChild(new Paragraph());
                        par.Append(new Run(new Text($"Середня вартість квитка - {(sum/(decimal)_context.Tickets.Count()).ToString("F2")} грн")));

                        if (_context.Concerts.Count() > 0)
                        {
                            int maxTickets = 0;
                            Concerts maxConcert = null;
                            foreach (Concerts c in _context.Concerts)
                            {
                                int count = _context.Tickets.Count(t => t.Sector.ConcertId == c.Id);
                                if (count > maxTickets)
                                {
                                    maxTickets = count;
                                    maxConcert = c;
                                }
                            }
                            par = body.AppendChild(new Paragraph());
                            par.Append(new Run(new Text($"Найпопулярніший концерт - {maxConcert.Name} (Квитки: {maxTickets})")));

                        }

                    }

                    mainPart.Document.Save();
                    wordDocument.Close();
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                    {
                        FileDownloadName = $"report_{DateTime.UtcNow.ToShortDateString()}.docx"
                    };
                }
            } 
        }

        public ActionResult Export(int? id)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                Countries country = _context.Countries.Where(c => c.Id == id).FirstOrDefault();

                var worksheet = workbook.Worksheets.Add(country.Name);
                worksheet.Cell("A1").Value = "Назва";
                worksheet.Cell("B1").Value = "Жанр 1";
                worksheet.Cell("C1").Value = "Жанр 2";
                worksheet.Cell("D1").Value = "Жанр 3";
                worksheet.Cell("E1").Value = "Жанр 4";
                worksheet.Cell("F1").Value = "Опис";
                worksheet.Row(1).Style.Font.Bold = true;

                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 12;
                worksheet.Column(3).Width = 12;
                worksheet.Column(4).Width = 12;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 60;

                var artists = _context.Artists.Where(a => a.CountryId == country.Id).ToList();
                //var artists = country.Artists.ToList();
                for (int i = 0; i < artists.Count; i ++)
                {
                    worksheet.Cell(i + 2, 1).Value = artists[i].Name;
                    worksheet.Cell(i + 2, 6).Value = artists[i].Description;
                    var ags = _context.ArtistsGenres.Where(ag => ag.ArtistId == artists[i].Id).Include("Genre").ToList();
                    
                    int j = 0;
                    foreach (var ag in ags)
                    {
                        if (j < 5)
                        {
                            worksheet.Cell(i + 2, j + 2).Value = ag.Genre.Name;
                            j++;
                        }
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"artists_{country.Name}_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
