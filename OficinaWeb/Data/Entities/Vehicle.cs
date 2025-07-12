using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class Vehicle : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "License Plate")]
        [MaxLength(6)]
        [RegularExpression(@"^\d{2}\s?[A-Z]{2}\s?\d{2}$", ErrorMessage = "License plate must be in format like 12HJ34 or 12 HJ 34.")]
        public string LicensePlate { get; set; }

        [Required]
        [Display(Name = "Car Brand")]
        public int CarBrandId { get; set; }

        
        public CarBrand CarBrand { get; set; }

        [Required]
        [Display(Name = "Car Model")]
        public int CarModelId { get; set; }

       
 
        public CarModel CarModel { get; set; }


        [Required]
        [Range(1920, 2040)]
        public int Year { get; set; }

        [Required]
        [Range(0, 2000000, ErrorMessage = "Mileage must be between 0 and 2,000,000.")]
        public int Mileage { get; set; }

        [Required]
        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [Display(Name = "Client")]
        [Required(ErrorMessage = "Client is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid client.")]
        public int ClientId { get; set; }

        public Client Client { get; set; }

        public User User { get; set; }

        public ICollection<Appointment> Appointments { get; set; }



        public string VehicleDescription => $"{CarBrand} {CarModel}, {LicensePlate}";



    }
}
