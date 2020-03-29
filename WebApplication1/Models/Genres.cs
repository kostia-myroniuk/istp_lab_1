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
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name="Назва")]
        public string Name { get; set; }

        public virtual ICollection<ArtistsGenres> ArtistsGenres { get; set; }
    }
}
