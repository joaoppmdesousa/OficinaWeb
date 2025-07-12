using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OficinaWeb.Data.Entities
{
    public class CarBrand : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
    }
}
