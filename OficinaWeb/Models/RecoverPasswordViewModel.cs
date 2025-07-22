using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
