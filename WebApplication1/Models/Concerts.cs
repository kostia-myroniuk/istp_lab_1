using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Concerts
    {
        public Concerts()
        {
            ConcertsArtists = new HashSet<ConcertsArtists>();
            Sectors = new HashSet<Sectors>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Info")]
        public string Description { get; set; }
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Display(Name = "Place")]
        public int LocationId { get; set; }

        [Display(Name = "Place")]
        public virtual Locations Location { get; set; }
        public virtual ICollection<ConcertsArtists> ConcertsArtists { get; set; }
        public virtual ICollection<Sectors> Sectors { get; set; }
    }
}
