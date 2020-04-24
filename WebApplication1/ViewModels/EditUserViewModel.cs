using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required]
        [Range(1900, 2020, ErrorMessage = "Year has to be from 1900 to 2020")]
        public int Year { get; set; }
    }
}
