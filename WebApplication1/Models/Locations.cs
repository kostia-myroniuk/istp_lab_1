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
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Адреса")]
        public string Adress { get; set; }
        [Display(Name = "Місто")]
        public int CityId { get; set; }

        [Display(Name = "Місто")]
        public virtual Cities City { get; set; }
        public virtual ICollection<Concerts> Concerts { get; set; }
    }
}
