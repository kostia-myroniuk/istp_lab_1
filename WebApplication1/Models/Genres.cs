using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Genres
    {
        public Genres()
        {
            ArtistsGenres = new HashSet<ArtistsGenres>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name="Name")]
        public string Name { get; set; }

        public virtual ICollection<ArtistsGenres> ArtistsGenres { get; set; }
    }
}
