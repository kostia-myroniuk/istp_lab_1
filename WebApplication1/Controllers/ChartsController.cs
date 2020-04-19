using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly ConcertsContext _context;

        public ChartsController(ConcertsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var countries = _context.Countries.Include(c => c.Artists).ToList();
            List<object> countriesArtists = new List<object>();
            countriesArtists.Add(new[] { "Country", "Artist amount" });
            foreach (var c in countries)
            {
                countriesArtists.Add(new object[] { c.Name, c.Artists.Count() });
            }
            return new JsonResult(countriesArtists);
        }

        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var sectors = _context.Sectors.Include(c => c.Concert).ToList();
            List<object> sectorsPrices = new List<object>();
            sectorsPrices.Add(new[] { "Concert/Sector", "Price" });
            foreach (var s in sectors)
            {
                sectorsPrices.Add(new object[] { s.Concert.Name + " / " + s.Name, s.Price });
            }
            return new JsonResult(sectorsPrices);
        }

        [HttpGet("JsonData3")]
        public JsonResult JsonData3()
        {
            var genres = _context.Genres.ToList();
            List<object> genresArtists = new List<object>();
            genresArtists.Add(new[] { "Genre", "Artist amount" });
            foreach (var g in genres)
            {
                genresArtists.Add(new object[] { g.Name, _context.ArtistsGenres.Where(ag => ag.GenreId == g.Id).Count() });
            }
            return new JsonResult(genresArtists);
        }

    }
}