using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Locations
    {
        public Locations()
        {
            Concerts = new HashSet<Concerts>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Adress")]
        public string Adress { get; set; }
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "City")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Concerts> Concerts { get; set; }
    }
}
