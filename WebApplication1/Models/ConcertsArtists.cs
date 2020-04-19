using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class ConcertsArtists
    {
        public int Id { get; set; }
        [Display(Name = "Concert")]
        public int ConcertId { get; set; }
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }

        [Display(Name = "Artist")]
        public virtual Artists Artist { get; set; }
        [Display(Name = "Concert")]
        public virtual Concerts Concert { get; set; }
    }
}
