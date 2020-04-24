using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        [Required]
        [Range(1900, 2020, ErrorMessage = "Year has to be from 1900 to 2020")]
        public int Year { get; set; }
    }
}
