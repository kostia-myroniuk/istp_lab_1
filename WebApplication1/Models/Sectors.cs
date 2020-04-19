using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Sectors
    {
        public Sectors()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name = "Sector")]
        public string Name { get; set; }
        [Display(Name = "Concert")]
        public int ConcertId { get; set; }

        [Required(ErrorMessage = "Field can't be empty")]
        [Range(0, 10000, ErrorMessage = "Price has to be an integer value between 0 and 10000")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Display(Name = "Concert")]
        public virtual Concerts Concert { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
