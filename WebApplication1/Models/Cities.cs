using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Cities
    {
        public Cities()
        {
            Locations = new HashSet<Locations>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Country")]
        public virtual Countries Country { get; set; }
        public virtual ICollection<Locations> Locations { get; set; }
    }
}
