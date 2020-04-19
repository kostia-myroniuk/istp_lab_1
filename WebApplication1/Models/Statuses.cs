using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Statuses
    {
        public Statuses()
        {
            Tickets = new HashSet<Tickets>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Field can't be empty")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
