using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Countries
    {
        public Countries()
        {
            Artists = new HashSet<Artists>();
            Cities = new HashSet<Cities>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<Artists> Artists { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
    }
}
