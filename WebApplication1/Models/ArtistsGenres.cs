using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class ArtistsGenres
    {
        public int Id { get; set; }
        [Display(Name = "Виконавець")]
        public int ArtistId { get; set; }
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }

        [Display(Name = "Виконавець")]
        public virtual Artists Artist { get; set; }
        [Display(Name = "Жанр")]
        public virtual Genres Genre { get; set; }
    }
}
