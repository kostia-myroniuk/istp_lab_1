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
            countriesArtists.Add(new[] { "Країна", "Кількість виконавців" });
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
            sectorsPrices.Add(new[] { "Концерт/Сектор", "Вартість" });
            foreach (var s in sectors)
            {
                sectorsPrices.Add(new object[] { s.Concert.Name + " / " + s.Name, s.Price });
            }
            return new JsonResult(sectorsPrices);
        }


    }
}