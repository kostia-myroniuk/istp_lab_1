using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class ArtistsGenres
    {
        public int Id { get; set; }
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Display(Name = "Artist")]
        public virtual Artists Artist { get; set; }
        [Display(Name = "Genre")]
        public virtual Genres Genre { get; set; }
    }
}
