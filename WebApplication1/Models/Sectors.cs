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
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Сектор")]
        public string Name { get; set; }
        [Display(Name = "Концерт")]
        public int ConcertId { get; set; }

        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Range(0, 10000, ErrorMessage = "Ціна має знаходитись в діапазоні від 0 до 10000")]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }

        [Display(Name = "Концерт")]
        public virtual Concerts Concert { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
    }
}
