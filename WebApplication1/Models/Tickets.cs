using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Tickets
    {
        public int Id { get; set; }
        [Display(Name = "Сектор")]
        public int SectorId { get; set; }
        [Display(Name = "Клієнт")]
        public int? ClientId { get; set; }
        [Display(Name = "Статус")]
        public int StatusId { get; set; }

        [Display(Name = "Клієнт")]
        public virtual Clients Client { get; set; }
        [Display(Name = "Сектор")]
        public virtual Sectors Sector { get; set; }
        [Display(Name = "Статус")]
        public virtual Statuses Status { get; set; }
    }
}
