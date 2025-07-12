using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class CarModel : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Display(Name = "Car Brand")]
        public int CarBrandId { get; set; }

        public CarBrand CarBrand { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}