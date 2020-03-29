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
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
        [Display(Name = "Країна")]
        public int CountryId { get; set; }

        [Display(Name = "Країна")]
        public virtual Countries Country { get; set; }
        public virtual ICollection<Locations> Locations { get; set; }
    }
}
