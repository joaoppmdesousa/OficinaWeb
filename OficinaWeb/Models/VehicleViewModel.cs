using OficinaWeb.Data.Entities;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class VehicleViewModel : Vehicle
    {
        public List<CarBrand> CarBrands { get; set; }

        public List<CarModel> CarModels { get; set; }

    }
}
