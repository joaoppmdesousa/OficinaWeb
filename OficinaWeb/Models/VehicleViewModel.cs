using Microsoft.AspNetCore.Mvc.Rendering;
using OficinaWeb.Data.Entities;
using System.Collections.Generic;

namespace OficinaWeb.Models
{
    public class VehicleViewModel : Vehicle
    {
        public List<CarBrand> CarBrands { get; set; }

        public List<CarModel> CarModels { get; set; }


        public SelectList FuelTypes { get; set; } = new SelectList(new[]
        {
        "Gasoline",
        "Diesel",
        "Electric",
        "Hybrid"
    });

    }
}
