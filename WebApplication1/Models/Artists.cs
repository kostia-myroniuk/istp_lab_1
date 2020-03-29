using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Artists
    {
        public Artists()
        {
            ArtistsGenres = new HashSet<ArtistsGenres>();
            ConcertsArtists = new HashSet<ConcertsArtists>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage ="Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Країна")]
        public int CountryId { get; set; }

        [Display(Name = "Країна")]
        public virtual Countries Country { get; set; }
        public virtual ICollection<ArtistsGenres> ArtistsGenres { get; set; }
        public virtual ICollection<ConcertsArtists> ConcertsArtists { get; set; }
    }
}
