using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Enter a valid phone number.")]
        public string Contact { get; set; }

        [Required]
        [Display(Name = "Tax Number")]
        [RegularExpression(@"^[0-9]{9,15}$", ErrorMessage = "Tax number must be between 9 and 15 digits.")]
        public string TaxNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }


        public User User { get; set; }





    }
}
