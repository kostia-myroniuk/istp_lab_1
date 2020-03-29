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
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Опис")]
        public string Description { get; set; }
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Місце")]
        public int LocationId { get; set; }

        [Display(Name = "Місце")]
        public virtual Locations Location { get; set; }
        public virtual ICollection<ConcertsArtists> ConcertsArtists { get; set; }
        public virtual ICollection<Sectors> Sectors { get; set; }
    }
}
