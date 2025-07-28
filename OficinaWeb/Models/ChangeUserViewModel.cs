using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Models
{
    public class ChangeUserViewModel
    {
        [Required]
        public string Name { get; set; }


        [Display(Name = "Profile picture")]
        public IFormFile? ImageFile { get; set; }  

        public string? ImageUrl { get; set; }


    }
}
