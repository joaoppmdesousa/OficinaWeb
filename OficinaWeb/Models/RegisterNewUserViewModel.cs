using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        public string Name { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public  string Password { get; set; }


        [Required]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
