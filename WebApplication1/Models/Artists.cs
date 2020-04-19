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
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Info")]
        public string Description { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public virtual Countries Country { get; set; }
        public virtual ICollection<ArtistsGenres> ArtistsGenres { get; set; }
        public virtual ICollection<ConcertsArtists> ConcertsArtists { get; set; }
    }
}
