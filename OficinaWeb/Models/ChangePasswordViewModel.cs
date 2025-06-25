using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name ="Current Password")]        
        public string OldPassword { get; set; }


        [Required]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
