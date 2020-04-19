using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public partial class Tickets
    {
        public int Id { get; set; }
        [Display(Name = "Sector")]
        public int SectorId { get; set; }
        [Display(Name = "Client")]
        public int? ClientId { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Client")]
        public virtual Clients Client { get; set; }
        [Display(Name = "Sector")]
        public virtual Sectors Sector { get; set; }
        [Display(Name = "Status")]
        public virtual Statuses Status { get; set; }
    }
}
