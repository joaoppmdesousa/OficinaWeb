using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Models
{
    public class CustomMessageViewModel
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
