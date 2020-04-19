using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication1.ViewModels
{
    public class ArtistsViewModel
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public List<string> Genres { get; set; }
        public Countries Country { get; set; }
    }
}
